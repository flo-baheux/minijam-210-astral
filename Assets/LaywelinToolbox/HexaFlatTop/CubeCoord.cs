#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Laywelin {
  public enum HexFlatTopDirection {
    UP,
    UP_LEFT,
    UP_RIGHT,
    DOWN_LEFT,
    DOWN_RIGHT,
    DOWN
  }

  [Serializable]
  public class CubeCoordData {
    public int q, r, s;

    public CubeCoord ToCoord() {
      return new(q, r, s);
    }
  }

  public static class VectorIntExtensions {
    public static CubeCoord ToCubeCoord(this Vector2Int pos) {
      return new(pos);
    }

    public static CubeCoord ToCubeCoord(this Vector3Int pos) {
      return new(pos);
    }
  }

  public readonly struct CubeCoord : IEquatable<CubeCoord> {
    public readonly int q;
    public readonly int r;
    public readonly int s;

    public CubeCoord(int q, int r, int s) {
      if (q + r + s != 0)
        throw new ArgumentException("Cube coordinates must sum to 0");
      this.q = q;
      this.r = r;
      this.s = s;
    }

    public CubeCoord(int q, int r) {
      this.q = q;
      this.r = r;
      s = -q - r;
    }

    public CubeCoord(Vector3Int pos) : this((Vector2Int)pos) { }

    public CubeCoord(Vector2Int pos) {
      // NB: In Unity X and Y are inverted for hexa flat top. y = columns, x = rows;

      // q = column
      q = pos.y;

      // r = row, but up is positive and down is negative
      r = -pos.x - (pos.y + (pos.y & 1)) / 2;
      s = -q - r;
    }

    public static CubeCoord operator +(CubeCoord a, CubeCoord b) {
      return new(a.q + b.q, a.r + b.r, a.s + b.s);
    }

    public static CubeCoord operator +(CubeCoord a, HexFlatTopDirection dir) {
      return a + directionCubeCoord[dir];
    }

    public static CubeCoord operator -(CubeCoord a, HexFlatTopDirection dir) {
      return a - directionCubeCoord[dir];
    }

    public static CubeCoord operator -(CubeCoord a, CubeCoord b) {
      return new(a.q - b.q, a.r - b.r, a.s - b.s);
    }

    public static bool operator ==(CubeCoord a, CubeCoord b) {
      return a.Equals(b);
    }

    public static bool operator !=(CubeCoord a, CubeCoord b) {
      return !a.Equals(b);
    }

    public bool Equals(CubeCoord other) {
      return q == other.q && r == other.r;
    }

    public override bool Equals(object? obj) {
      return obj is CubeCoord other && Equals(other);
    }

    public override int GetHashCode() {
      return HashCode.Combine(q, r);
    }

    public override string ToString() {
      return $"CubeCoord ({q}, {r}, {s})";
    }

    public Vector2Int ToVector2Int() {
      var col = q;
      var row = r + (q + (q & 1)) / 2;
      // In Unity X and Y are inverted for hexa flat top, and up is positive/down is negative
      return new(-row, col);
    }

    public Vector3Int ToVector3Int() {
      return (Vector3Int)ToVector2Int();
    }

    public static readonly Dictionary<HexFlatTopDirection, CubeCoord> directionCubeCoord = new() {
      { HexFlatTopDirection.UP, new(0, -1, 1) },
      { HexFlatTopDirection.UP_LEFT, new(-1, 0, 1) },
      { HexFlatTopDirection.UP_RIGHT, new(1, -1, 0) },
      { HexFlatTopDirection.DOWN_LEFT, new(-1, 1, 0) },
      { HexFlatTopDirection.DOWN_RIGHT, new(1, 0, -1) },
      { HexFlatTopDirection.DOWN, new(0, 1, -1) }
    };

    public static HexFlatTopDirection Opposite(HexFlatTopDirection of) {
      return of switch {
        HexFlatTopDirection.UP => HexFlatTopDirection.DOWN,
        HexFlatTopDirection.UP_RIGHT => HexFlatTopDirection.DOWN_LEFT,
        HexFlatTopDirection.DOWN_RIGHT => HexFlatTopDirection.UP_LEFT,
        HexFlatTopDirection.DOWN => HexFlatTopDirection.UP,
        HexFlatTopDirection.DOWN_LEFT => HexFlatTopDirection.UP_RIGHT,
        HexFlatTopDirection.UP_LEFT => HexFlatTopDirection.DOWN_RIGHT,
        _ => throw new ArgumentOutOfRangeException(nameof(of), of, null)
      };
    }

    public static HexFlatTopDirection ClockNext(HexFlatTopDirection of) {
      return of switch {
        HexFlatTopDirection.UP => HexFlatTopDirection.UP_RIGHT,
        HexFlatTopDirection.UP_RIGHT => HexFlatTopDirection.DOWN_RIGHT,
        HexFlatTopDirection.DOWN_RIGHT => HexFlatTopDirection.DOWN,
        HexFlatTopDirection.DOWN => HexFlatTopDirection.DOWN_LEFT,
        HexFlatTopDirection.DOWN_LEFT => HexFlatTopDirection.UP_LEFT,
        HexFlatTopDirection.UP_LEFT => HexFlatTopDirection.UP,
        _ => throw new ArgumentOutOfRangeException(nameof(of), of, null)
      };
    }

    public static HexFlatTopDirection ClockPrevious(HexFlatTopDirection of) {
      return of switch {
        HexFlatTopDirection.UP => HexFlatTopDirection.UP_LEFT,
        HexFlatTopDirection.UP_LEFT => HexFlatTopDirection.DOWN_LEFT,
        HexFlatTopDirection.DOWN_LEFT => HexFlatTopDirection.DOWN,
        HexFlatTopDirection.DOWN => HexFlatTopDirection.DOWN_RIGHT,
        HexFlatTopDirection.DOWN_RIGHT => HexFlatTopDirection.UP_RIGHT,
        HexFlatTopDirection.UP_RIGHT => HexFlatTopDirection.UP,
        _ => throw new ArgumentOutOfRangeException(nameof(of), of, null)
      };
    }

    public static CubeCoord zero = new(0, 0, 0);
    public static CubeCoord up = directionCubeCoord[HexFlatTopDirection.UP];
    public static CubeCoord upLeft = directionCubeCoord[HexFlatTopDirection.UP_LEFT];
    public static CubeCoord upRight = directionCubeCoord[HexFlatTopDirection.UP_RIGHT];
    public static CubeCoord downLeft = directionCubeCoord[HexFlatTopDirection.DOWN_LEFT];
    public static CubeCoord downRight = directionCubeCoord[HexFlatTopDirection.DOWN_RIGHT];
    public static CubeCoord down = directionCubeCoord[HexFlatTopDirection.DOWN];
    public static CubeCoord MaxValue = new(9999999, -9999999, 0);
    public static CubeCoord MinValue = new(-9999999, 9999999, 0);


    public static bool IsABelowB(CubeCoord a, CubeCoord b, bool includeSides = false) {
      return (includeSides && a.s == b.s && a.q < b.q) || (a.q == b.q && a.s < b.s) ||
             (includeSides && a.r == b.r && a.s < b.s);
    }

    public static HexFlatTopDirection GetDirectionAToB(CubeCoord a, CubeCoord b) {
      if (a == b || a.DistanceWith(b) > 1)
        throw new InvalidOperationException($"Cannot get direction between {a} and {b}");

      if (a + up == b)
        return HexFlatTopDirection.UP;
      if (a + upRight == b)
        return HexFlatTopDirection.UP_RIGHT;
      if (a + downRight == b)
        return HexFlatTopDirection.DOWN_RIGHT;
      if (a + down == b)
        return HexFlatTopDirection.DOWN;
      if (a + downLeft == b)
        return HexFlatTopDirection.DOWN_LEFT;
      if (a + upLeft == b)
        return HexFlatTopDirection.UP_LEFT;

      return HexFlatTopDirection.UP;
    }

    private static HexFlatTopDirection GetRandomDirection() {
      return (HexFlatTopDirection)Enum
        .GetValues(typeof(HexFlatTopDirection))
        .GetValue(Random.Range(0, Enum.GetNames(typeof(HexFlatTopDirection)).Length));
    }

    public CubeCoord GetRandomAdjacent() {
      return this + directionCubeCoord[GetRandomDirection()];
    }

    public List<CubeCoord> GetAllNeighbors(int distance = 1) {
      List<CubeCoord> results = new();

      for (var q = -distance; q <= distance; q++)
        for (var r = Math.Max(-distance, -q - distance); r <= Math.Min(distance, -q + distance); r++)
          results.Add(this + new CubeCoord(q, r, -q - r));

      return results;
    }

    public int DistanceWith(CubeCoord other) {
      var vector = this - other;
      return (Math.Abs(vector.q) + Math.Abs(vector.r) + Math.Abs(vector.s)) / 2;
    }

    public static int DistanceBetween(CubeCoord a, CubeCoord b) {
      return a.DistanceWith(b);
    }

    public CubeCoordData Serialize() {
      return new() {
        q = q,
        r = r,
        s = s
      };
    }

    public static CubeCoord GetMousePosCubeCoord(Tilemap tilemap) {
      var mouseScreenPos = Mouse.current.position.ReadValue();
      Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
      return WorldPosToCubeCoord(tilemap, mouseWorldPos);
    }

    public static CubeCoord WorldPosToCubeCoord(Tilemap tilemap, Vector2 worldPos) {
      return tilemap.WorldToCell(worldPos).ToCubeCoord();
    }

    public static Vector2 CubeCoordToWorldPos(Tilemap tilemap, CubeCoord coordinates) {
      return tilemap.CellToWorld(coordinates.ToVector3Int());
    }
  }
}