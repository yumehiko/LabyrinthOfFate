using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AStar.Collections.PathFinder;
using AStar.Heuristics;
using AStar.Options;
using UnityEngine;

namespace AStar
{
    public partial class PathFinder
    {
        ///<inheritdoc/>
        public Vector2Int[] FindPath(Vector2Int start, Vector2Int end)
        {
            return FindPath(new Position(start.x, start.y), new Position(end.x, end.y))
                .Select(position => new Vector2Int(position.Row, position.Column))
                .ToArray();
        }
    }
}