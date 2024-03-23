using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Steamworks;
using UnityEngine;

public class SteamWorkshopDownloader : MonoBehaviour {
    private static SteamWorkshopDownloader instance;
    private CallResult<SteamUGCQueryCompleted_t> queryCallResult;
    private Callback<DownloadItemResult_t> downloadResult;
    private bool waitingOnDownloads;

    public delegate void StatusChangedAction(SteamWorkshopDownloader downloader, Status status);

    public static event StatusChangedAction statusChanged;

    public enum StatusType {
        Working,
        Done,
        Error,
    }

    private Status status = new() {
        message = "Booting up...",
        progress = 0f,
        statusType = StatusType.Working,
    };
    public struct Status {
        public string message;
        public float progress;
        public StatusType statusType;
    }

    private void SetStatus(string message, float progress, StatusType statusType) {
        status = new Status {
            message = message,
            progress = progress,
            statusType = statusType
        };
        statusChanged?.Invoke(this, status);
    }

    public static Status GetStatus() {
        if (instance == null) {
            return new Status() {
                message = "Booting up...",
                progress = 0f,
                statusType = StatusType.Working,
            };
        }
        return instance.status;
    }

    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private IEnumerator Start() {
        yield return new WaitUntil(() => SteamManager.Initialized || SteamManager.FailedToInitialize);
        if (!SteamUser.BLoggedOn() || SteamManager.FailedToInitialize) {
            SetStatus("User not logged into Steam, cannot use workshop!", 1f, StatusType.Done);
            Debug.LogError(status.message);
            yield break;
        }
        var subscribedItemCount = SteamUGC.GetNumSubscribedItems();
        if(subscribedItemCount == 0) {
            SetStatus("Successfully installed all subscribed items.", 1f, StatusType.Done); 
            yield break;
        }
        var subscribedIDs = new PublishedFileId_t[subscribedItemCount];
        uint populatedCount = SteamUGC.GetSubscribedItems(subscribedIDs, subscribedItemCount);
        var queryRequest = SteamUGC.CreateQueryUGCDetailsRequest(subscribedIDs, populatedCount);
        queryCallResult?.Dispose();
        queryCallResult = new CallResult<SteamUGCQueryCompleted_t>(OnQueryCallResult);
        var queryHandle = SteamUGC.SendQueryUGCRequest(queryRequest);
        queryCallResult.Set(queryHandle);
        downloadResult?.Dispose();
        downloadResult = new Callback<DownloadItemResult_t>(OnDownloadResult);
    }
    private void OnQueryCallResult(SteamUGCQueryCompleted_t param, bool biofailure) {
        for (uint i = 0; i < param.m_unNumResultsReturned; i++) {
            if (!SteamUGC.GetQueryUGCResult(param.m_handle, i, out var details)) {
                Debug.LogError("Skipped malformed SteamWorkshop item.");
                continue;
            }

            uint statusFlags = SteamUGC.GetItemState(details.m_nPublishedFileId);
            if ((statusFlags & (uint)EItemState.k_EItemStateInstalled) == 0 || (statusFlags & (uint)EItemState.k_EItemStateNeedsUpdate) != 0) {
                if (SteamUGC.DownloadItem(details.m_nPublishedFileId, true)) {
                    Debug.Log($"Downloading Steamworks item {details.m_rgchTitle} {details.m_nPublishedFileId}");
                } else {
                    Debug.LogError($"Couldn't download Steam workshop item {details.m_rgchTitle} {details.m_nPublishedFileId} because the user is not logged in, or the file is invalid...");
                }
            } else {
                LoadMod(details.m_nPublishedFileId);
            }
        }
        StartCoroutine(WaitOnDownloads(param));
    }

    private void OnDownloadResult(DownloadItemResult_t param) {
        if (param.m_unAppID != SteamUtils.GetAppID()) {
            return;
        }
        if (param.m_eResult != EResult.k_EResultOK) {
            Debug.LogError($"Failed to download Steam workshop item {param.m_nPublishedFileId} with status {param.m_eResult}.");
            return;
        }
        LoadMod(param.m_nPublishedFileId);
    }

    private IEnumerator WaitOnDownloads(SteamUGCQueryCompleted_t param) {
        for (uint i = 0; i < param.m_unNumResultsReturned; i++) {
            if (!SteamUGC.GetQueryUGCResult(param.m_handle, i, out var details)) {
                Debug.LogError("Skipped malformed Steam workshop item.");
                continue;
            }
            uint statusFlags = SteamUGC.GetItemState(details.m_nPublishedFileId);
            while ((statusFlags & (int)EItemState.k_EItemStateNeedsUpdate) != 0) {
                if (!SteamUGC.GetItemDownloadInfo(details.m_nPublishedFileId, out ulong bytesDownloaded, out ulong bytesTotal)) {
                    break;
                }
                SetStatus($"Downloading {details.m_rgchTitle}...", (float)bytesDownloaded/bytesTotal, StatusType.Working);
                statusFlags = SteamUGC.GetItemState(details.m_nPublishedFileId);
                yield return null;
            }
        }
        SetStatus("Successfully installed all subscribed items.", 1f, StatusType.Done);
    }

    private void LoadMod(PublishedFileId_t itemID) {
        bool hasData = SteamUGC.GetItemInstallInfo(itemID, out ulong punSizeOnDisk, out string pchFolder, 1024, out uint punTimeStamp);
        if (!hasData) {
            Debug.LogError($"No data for SteamWorkshop ID {itemID}!");
            return;
        }
        Debug.Log($"Loaded SteamWorks item {itemID}");
        DirectoryInfo dirInfo = new DirectoryInfo(pchFolder);
        Modding.AddMod(new LocalMod(dirInfo));
    }
}
