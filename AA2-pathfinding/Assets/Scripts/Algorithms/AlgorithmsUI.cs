using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AI {

    public class AlgorithmsUI : MonoBehaviour {
        public Agent agent;
        public TMP_Dropdown optionList;

        private List<IAlgorithm> algorithms;

        private void Start() {
            algorithms = new List<IAlgorithm>() { new BFS(), new Dijkstra(), new GreedyBFS(), new Astar() };

            optionList.options.Clear();

            var opts = new List<TMP_Dropdown.OptionData>();
            foreach(IAlgorithm alg in algorithms) {
                opts.Add(new TMP_Dropdown.OptionData(alg.Name));
                Debug.Log(opts);
            }

            optionList.AddOptions(opts);
            optionList.RefreshShownValue();

            optionList.onValueChanged.AddListener(i => {
                agent.SetAlgorithm(algorithms[i]);
            });

            agent.SetAlgorithm(algorithms[0]);
        }
    }
}