using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AI {

    public class AlgorithmsUI : MonoBehaviour {
        public Agent agent;
        public TMP_Dropdown optionList;
        public TMP_InputField inputField;
        public Button button;

        private List<IAlgorithm> algorithms;
        private Vector2Int goalPosition;

        private void Start() {
            algorithms = new List<IAlgorithm>() {
                new BFS(),
                new Dijkstra(),
                new GreedyBFS(),
                new Astar()
            };

            optionList.options.Clear();
            var opts = new List<TMP_Dropdown.OptionData>();

            foreach(IAlgorithm alg in algorithms) {
                opts.Add(new TMP_Dropdown.OptionData(alg.Name));
            }

            optionList.AddOptions(opts);
            optionList.RefreshShownValue();

            optionList.onValueChanged.AddListener(i => {
                agent.SetAlgorithm(algorithms[i]);
            });

            inputField.onValueChanged.AddListener(text => {
                Vector2Int parsed;

                if(TryParseVector2Int(text, out parsed))
                    goalPosition = parsed;
            });

            agent.SetAlgorithm(algorithms[0]);

            button.onClick.AddListener(OnClickButton);
        }

        private bool TryParseVector2Int(string text, out Vector2Int result) {
            result = Vector2Int.zero;

            if(string.IsNullOrWhiteSpace(text))
                return false;

            string[] parts = text.Split(',');

            if(parts.Length != 2)
                return false;

            if(int.TryParse(parts[0], out int x) &&
                int.TryParse(parts[1], out int y)) {
                result = new Vector2Int(x, y);
                return true;
            }

            return false;
        }

        private void OnClickButton() {
            agent.SetAlgorithm(algorithms[optionList.value]);
            agent.MoveTo(goalPosition);
        }
    }
}