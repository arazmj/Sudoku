using System;
using System.Collections.Generic;
using System.Linq;

namespace Sodoku
{
	class MainClass
	{
		const int size = 9;

		static int[] GetRow (int r, int[,] values)
		{
			int[] row = new int[size];
			for (int i = 0; i < size; i++)
				row [i] = values [r, i];
			return row;		
		}

		static int[] GetColumn(int c, int[,] values) {
			int[] column = new int[size];
			for (int i = 0; i < size; i++)
				column [i] = values [i, c];
			return column;
		}

		static int[] GetBox (int r, int c, int[,] values)
		{
			int boxsize = (int)Math.Sqrt (size);
			int br = (r / boxsize) * boxsize;
			int bc = (c / boxsize) * boxsize;
			int[] box = new int[size];
			for (int i = 0; i < boxsize; i++)
				for (int j = 0; j < boxsize; j++) {
					box [i * boxsize + j] = 
						values [br + i, bc  + j];
					}
			return box;
		}

		static int[] GetPossibilities (int r, int c, int[,] values)
		{
			int[] possibilities = 
				Enumerable.Range (1, size)
					.Except (GetBox (r, c, values)
				.Union (GetRow (r, values))
				.Union (GetColumn (c, values)))
					.ToArray ();
			return possibilities;
		}

		static void dumpBoard (int[,] values)
		{
			for (int i = 0; i < size; i++) {
				for (int j = 0; j < size; j++) {
					if (values [i, j] == 0)
						Console.Write (" X");
					else
						Console.Write (string.Format ("{0,2}", values [i, j]));
				}
				Console.WriteLine ();
			}
		}


		static  LinkedList<State> computeStates (int[,] values)
		{
			var states = new LinkedList<State> ();
			for (int r = 0; r < size; r++) {
				for (int c = 0; c < size; c++) {
					if (values [r, c] != 0)
						continue;
					states.AddLast (
						new State () {
							Possibilities = GetPossibilities (r, c, values).ToList (),
							Position = new Position () {
								Row = r,
								Column = c
							}
					});
				}
			}
			return states;
		}

		class Position {
			public int Row { get; set; }
			public int Column { get; set; }
			public override int GetHashCode() {
				return Row * size + Column;
			}
			public override string ToString ()
			{
				return string.Format ("{0}:{1}", Row, Column);
			}
		}


		class State {
			public List<int> Possibilities { get; set; }
			public Position Position { get; set; }
		}

		public static void Main (string[] args)
		{
			int[,] values = new int[,]  {
				{0, 8, 6, 5, 0, 4, 0, 0, 1},
				{0, 0, 4, 0, 2, 0, 0, 7, 0},
				{0, 0, 3, 1, 0, 0, 0, 0, 4},
				{0, 5, 0, 7, 3, 9, 4, 0, 8},
				{3, 0, 0, 0, 0, 0, 0, 0, 2},
				{4, 0, 8, 6, 1, 2, 0, 9, 0},
				{6, 0, 0, 0, 0, 7, 1, 0, 0},
				{0, 4, 0, 0, 9, 0, 5, 0, 0},
				{9, 0, 0, 8, 0, 1, 6, 4, 0}};

			do {
				LinkedList<State> states = computeStates (values);
				if (states.Count == 0)
					break;
				int minSize = states.Select(p => p.Possibilities.Count).Min();
				foreach (var s in states.Where (p => p.Possibilities.Count == minSize)) {
					values [s.Position.Row, s.Position.Column] = s.Possibilities.First ();
				}
			} while (true);

			dumpBoard(values);

		}
	}
}
