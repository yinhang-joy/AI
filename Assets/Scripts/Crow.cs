using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


namespace System.Collections
{
    public interface Crow
    {


        object Current { get; }

        bool MoveNext();
        void Reset();
    }

}