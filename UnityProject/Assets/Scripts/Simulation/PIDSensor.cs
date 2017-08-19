/// Author: Samuel Arzt
/// Date: March 2017

#region Includes
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#endregion

/// <summary>
/// Class representing a sensor reading the distance to the nearest obstacle in a specified direction.
/// </summary>
public class PIDSensor : Sensor
{
    #region Members
    const int MAX_INTEGRATE = 30;
    public float OutputI { get;private set; } // added an integrative factor
    public float OutputD { get;private set; } // added an derivative factor
    private FixedSizedQueue<float> Others;
    public override double[] GetValues { get { return new double[] { Output, OutputI, OutputD }; } } //give values through array

    #endregion

    #region Constructors
    internal override void Init()
    {
        base.Init();
        Others = new FixedSizedQueue<float>(MAX_INTEGRATE);
    }
    #endregion

    #region Methods
    // Unity method for updating the simulation
    internal override void SetInnerValues(Vector2 direction, float Distance)
    {
        base.SetInnerValues(direction, Distance);
        if (Others.Innerq.Any()) OutputD = Output - Others.LastValue;
        Others.Enqueue(Output);
        OutputI = Others.Innerq.Average();
    }
    #endregion

    public class FixedSizedQueue<T>
    {
        public FixedSizedQueue(int Lim)
        {
            Limit = Lim;
        }
        public Queue<T> Innerq = new Queue<T>();
        public int Limit { get; set; }
        public T LastValue { get; internal set; }
        public void Enqueue(T obj)
        {
            LastValue = obj;
            Innerq.Enqueue(obj);
            while (Innerq.Count > Limit) Innerq.Dequeue() ;
        }
    }
}
