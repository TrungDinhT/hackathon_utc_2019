using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Positionning
{
	public class Position : MonoBehaviour
	{
		private double x;
		private double y;

		public Position(double x1, double y1)
		{
			x = x1;
			y = y1;
		}

		public double getX()
		{
			return x;
		}

		public double getY()
		{
			return y;
		}
	}

}
