using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Metroidvania.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        [Inject] private SkillManager skillManager;

    }

}
