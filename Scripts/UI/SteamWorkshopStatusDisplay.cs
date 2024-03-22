using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class SteamWorkshopStatusDisplay : MonoBehaviour {
    [SerializeField] private ProgressCircle progressCircle;
    [SerializeField] private TMPro.TMP_Text statusText;
    [SerializeField] private TMPro.TMP_Text extraErrorText;
    [SerializeField] private LocalizedString extraErrorString;
    [SerializeField] private Animator animator;
    private static readonly int Active = Animator.StringToHash("Active");

    void OnEnable() {
        SteamWorkshopDownloader.statusChanged += OnStatusChanged;
        OnStatusChanged(null, SteamWorkshopDownloader.GetStatus());
    }

    private void OnDisable() {
        SteamWorkshopDownloader.statusChanged -= OnStatusChanged;
    }

    private void OnStatusChanged(SteamWorkshopDownloader downloader, SteamWorkshopDownloader.Status status) {
        statusText.text = status.message;
        progressCircle.SetProgress(status.progress);
        animator.SetBool(Active, status.statusType != SteamWorkshopDownloader.StatusType.Done);
        if (status.statusType == SteamWorkshopDownloader.StatusType.Error) {
            progressCircle.SetColor(Color.red);
            extraErrorText.text = extraErrorString.GetLocalizedString();
            var e = extraErrorText.GetComponent<LocalizeStringEvent>();
            if (e != null) {
                e.StringReference = extraErrorString;
            }
        }
    }
}
