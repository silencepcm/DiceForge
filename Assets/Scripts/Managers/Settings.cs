using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{

    [Header("Plateau")]
    public Dictionary<string, Dictionary<int, float>> plateauFirstCase = new Dictionary<string, Dictionary<int, float>>(4);
    public Dictionary<string, Dictionary<int, float>> plateauCase = new Dictionary<string, Dictionary<int, float>>(4);

    public Settings()
    {
        plateauFirstCase = new Dictionary<string, Dictionary<int, float>>(4)
        {
            { "Noir",
                new Dictionary<int, float>() {
                    { 1, 82f },
                    { 2, 100f },
                    { 3, 89f },
                    { 4, 42f },
                    { 5, 42f },
                }
            },
            { "Bleu",
                new Dictionary<int, float>() {
                    { 1, 82f },
                    { 2, 100f },
                    { 3, 88f },
                    { 4, 42f },
                    { 5, 42f },
                }
            },
            { "Rouge",
                new Dictionary<int, float>() {
                    { 1, 82f },
                    { 2, 100f },
                    { 3, 88f },
                    { 4, 42f },
                    { 5, 42f },
                }
            },
            { "Vert",
                new Dictionary<int, float>() {
                    { 1, 82f },
                    { 2, 100f },
                    { 3, 88f },
                    { 4, 42f },
                    { 5, 42f },
                }
            }
        };



        plateauCase = new Dictionary<string, Dictionary<int, float>>(4)
        {
            { "Noir",
                new Dictionary<int, float>() {
                    { 1, 39f },
                    { 2, 52f },
                    { 3, 52f },
                    { 4, 40f },
                    { 5, 40f },
                }
            },
            { "Bleu",
                new Dictionary<int, float>() {
                    { 1, 39f },
                    { 2, 52f },
                    { 3, 52f },
                    { 4, 40f },
                    { 5, 40f },
                }
            },
            { "Rouge",
                new Dictionary<int, float>() {
                    { 1, 39f },
                    { 2, 52f },
                    { 3, 52f },
                    { 4, 40f },
                    { 5, 40f },
                }
            },
            { "Vert",
                new Dictionary<int, float>() {
                    { 1, 39f },
                    { 2, 52f },
                    { 3, 52f },
                    { 4, 40f },
                    { 5, 40f },
                }
            }
        };

    }
}
