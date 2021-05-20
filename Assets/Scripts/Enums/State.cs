using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Menu,
    Start, // ожидание, пока не нарисуешь руки
    PrePlay, // обратный отсчет 123...
    Retry, // 
    Play,
    Finish,
    NextLevel
}
