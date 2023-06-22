using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommanOfDamini.Enum
{
    #region DIRECTION_RELATED
    public enum Directions2D { onSamePos, left, right, up, down, count };
    public enum Directions { onSamePos, left, right, up, down, forward, back, count };
    #endregion

    #region MEASURMENT_RELATED
    public enum MeasurmentFactorsOf3DObj { none, width, height, depth, count };
    public enum MeasurmentFactorsOf2DObj { none, width, height, count };
    #endregion

    #region SIGN_RELATED
    public enum Sign { neutral = 0, positive = 1, negative = -1 };
    #endregion

    #region TOGGLE_RELATED
    public enum Toggle { no, yes };
    #endregion
}

