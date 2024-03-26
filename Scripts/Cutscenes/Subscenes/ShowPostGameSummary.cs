using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Cutscenes.Subscenes {
    [Serializable]
    public class ShowPostGameSummary : Subscene {
        [FormerlySerializedAs("newRecord")] [SerializeField] private GameObject newRecordDisplay;
        [SerializeField] private TMP_Text objectivesText;
        [SerializeField] private TMP_Text oldObjectivesText;
        [SerializeField] private GameObject debriefObj;
        [SerializeField] private TMP_Text debriefText;
        [SerializeField] private LocalizedString completionTimeString;
        [SerializeField] private LocalizedString objectivesCompleted;
        [SerializeField] private Image mapIcon;
        [SerializeField] private TMP_Text mapLabel;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private DialogueTheme debriefTheme;

        private StringBuilder builder;
        private StringBuilder oldBuilder;
        private Level level;
        private WaitForSecondsRealtime interval;
        private double newCompletionTime;
        private bool gotNewRecord;

        protected override void OnStart() {
            base.OnStart();
            newCompletionTime = LevelManager.GetLevelTimer();
            interval = new WaitForSecondsRealtime(0.1f);
            debriefObj.SetActive(false);
            builder = new StringBuilder();
            builder.Append($"{completionTimeString.GetLocalizedString()}: <space=0.5em>\n");
            builder.Append($"{objectivesCompleted.GetLocalizedString()}\n");
            int objectiveNum = 0;
            foreach (var objective in ObjectivesDescription.GetObjectives()) {
                builder.Append($"\t<size=75%>{objective.GetLocalizedLabel().GetLocalizedString()}:</size> <space={objectiveNum.ToString()}em>\n");
                objectiveNum++;
            }

            objectivesText.maxVisibleCharacters = 0;
            objectivesText.text = builder.ToString();
            level = LevelManager.GetCurrentLevel();
            mapIcon.sprite = level.GetLevelPreview();
            mapLabel.text = level.GetLevelName();

            oldBuilder = new StringBuilder();
            level.GetCompletionStatus(out bool oldCompleted, out var oldObjectives, out var oldCompletionTime);
            if (oldCompleted) {
                oldBuilder.Append($"{completionTimeString.GetLocalizedString()}: {TimeSpan.FromSeconds(oldCompletionTime)}\n");
                oldBuilder.Append($"{objectivesCompleted.GetLocalizedString()}\n");
                foreach (var objective in oldObjectives) {
                    string completionStatus = objective.status switch {
                        Objective.ObjectiveStatus.Failed => "<color=red>X</color>",
                        Objective.ObjectiveStatus.Incomplete => "<color=grey>~</color>",
                        Objective.ObjectiveStatus.Completed => "<color=green>✓</color>",
                        _ => ""
                    };
                    oldBuilder.Append($"\t<size=75%>{objective.name.GetLocalizedString()}:</size> {completionStatus}\n");
                }
            }

            oldObjectivesText.text = oldBuilder.ToString();
            oldObjectivesText.maxVisibleCharacters = objectivesText.maxVisibleCharacters;
            gotNewRecord = level.SetCompletionStatus(true, ObjectivesDescription.GetObjectives(), newCompletionTime);
        }

        protected override IEnumerator Update() {
            newRecordDisplay.SetActive(false);
            
            float startTime = Time.unscaledTime;
            float duration = 1f;
            while (Time.unscaledTime < startTime + duration) {
                float t = (Time.unscaledTime - startTime) / duration;
                canvasGroup.alpha = t;
                yield return null;
            }
            canvasGroup.alpha = 1f;
            
            yield return new WaitForSeconds(1f);
            
            // Print out the finish level template
            startTime = Time.unscaledTime;
            while (Time.unscaledTime < startTime + duration) {
                float t = (Time.unscaledTime - startTime) / duration;
                objectivesText.maxVisibleCharacters = Mathf.RoundToInt(objectivesText.text.Length*t);
                oldObjectivesText.maxVisibleCharacters = objectivesText.maxVisibleCharacters;
                yield return null;
            }
            objectivesText.maxVisibleCharacters = objectivesText.text.Length+1000;
            oldObjectivesText.maxVisibleCharacters = objectivesText.maxVisibleCharacters;
            yield return new WaitForSeconds(1f);

            builder.Replace("<space=0.5em>", $"<color=green>{TimeSpan.FromSeconds(newCompletionTime)}</color>");
            objectivesText.text = builder.ToString();
            
            yield return new WaitForSeconds(1f);
            int objectiveNum = 0;
            foreach (var objective in ObjectivesDescription.GetObjectives()) {
                string completionStatus = objective.GetStatus() switch {
                    Objective.ObjectiveStatus.Failed => "<color=red>X</color>",
                    Objective.ObjectiveStatus.Incomplete => "<color=grey>~</color>",
                    Objective.ObjectiveStatus.Completed => "<color=green>✓</color>",
                    _ => ""
                };

                builder.Replace($"<space={objectiveNum.ToString()}em>", $"{completionStatus}");
                objectivesText.text = builder.ToString();
                objectiveNum++;
                yield return new WaitForSeconds(0.5f);
            }

            if (gotNewRecord) {
                newRecordDisplay.SetActive(true);
            }
            yield return new WaitForSeconds(1f);
            debriefText.text = ObjectivesDescription.GetDebrief();
            debriefText.maxVisibleCharacters = 0;
            debriefObj.SetActive(true);
            
            startTime = Time.unscaledTime;
            duration = debriefText.text.Length * 0.04f;
            while (Time.unscaledTime < startTime + duration) {
                float t = (Time.unscaledTime - startTime) / duration;
                debriefText.maxVisibleCharacters = Mathf.RoundToInt(t*debriefText.text.Length);
                AudioPack.PlayClipAtPoint(debriefTheme.GetTalkPack(), OrbitCamera.GetPlayerIntendedPosition());
                yield return interval;
            }
            debriefText.maxVisibleCharacters = debriefText.text.Length + 1000;
            yield return new WaitForSeconds(4f);
        }

        public override void OnEnd() {
            base.OnEnd();
            level.GetCompletionStatus(out var completed, out var levelObjectives, out var completionTime);
            // Then one by one replace marks with info
            builder.Replace("<timemark>", $"{TimeSpan.FromSeconds(LevelManager.GetLevelTimer())}");
            int objectiveNum = 0;
            foreach (var objective in ObjectivesDescription.GetObjectives()) {
                string completionStatus = objective.GetStatus() switch {
                    Objective.ObjectiveStatus.Failed => "<color=red>X</color>",
                    Objective.ObjectiveStatus.Incomplete => "<color=grey>~</color>",
                    Objective.ObjectiveStatus.Completed => "<color=green>✓</color>",
                    _ => ""
                };

                builder.Replace($"<objmark{objectiveNum.ToString()}>", $"{completionStatus}");
                objectiveNum++;
            }
            objectivesText.text = builder.ToString();
            objectivesText.maxVisibleCharacters = objectivesText.text.Length + 1000;
            oldObjectivesText.maxVisibleCharacters = objectivesText.maxVisibleCharacters;
            if (gotNewRecord) {
                newRecordDisplay.SetActive(true);
            }
            debriefObj.SetActive(true);
            debriefText.text = ObjectivesDescription.GetDebrief();
            debriefText.maxVisibleCharacters = debriefText.text.Length + 1000;
        }
    }
}
