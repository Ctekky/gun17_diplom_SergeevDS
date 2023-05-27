using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Metroidvania.Managers;

namespace Metroidvania.Managers
{
    public class SkillManager : MonoBehaviour
    {
        [Inject] private PlayerManager playerManager;
    }

}
