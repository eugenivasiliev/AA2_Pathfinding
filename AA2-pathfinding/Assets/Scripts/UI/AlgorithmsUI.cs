using System.Collections.Generic;
using AI.Algorithms;
using AI.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AI.UI {

    public class AlgorithmsUI : MonoBehaviour {

        [Header("References")]
        [SerializeField] private Agent agent;

        [SerializeField] private Grid grid;

        [SerializeField] public MultiDestinationAgent multiAgent;

        [Header("UI")]
        [SerializeField] private TMP_Dropdown optionList;

        [SerializeField] private Button clearVisualsButton;
        
        [SerializeField] private Button optimizeButton;

        [SerializeField] private Button multipathButton;

        private List<IAlgorithm> algorithms;

        private void Start() {
            algorithms = new List<IAlgorithm>() { new BFS(), new GreedyBFS(), new Dijkstra(), new Astar(), new DstarLite() };

            optionList.options.Clear();
            foreach(var alg in algorithms) optionList.options.Add(new TMP_Dropdown.OptionData(alg.Name));
            optionList.RefreshShownValue();

            optionList.onValueChanged.AddListener(i => {
                agent.SetAlgorithm(algorithms[i]);
            });

            agent.SetAlgorithm(algorithms[optionList.value]);
        }
    }
}