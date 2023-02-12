using System.Collections.Generic;
using UnityEngine;

namespace _CodeBase.Data
{
  [CreateAssetMenu(fileName = "SfxData", menuName = "StaticData/Sfx")]
  public class SfxData : ScriptableObject
  {
    public List<AudioClip> HeroDamage;
    [Space(10)]
    public List<AudioClip> HeroDeath;
    [Space(10)] 
    public List<AudioClip> RockDestroy;
    [Space(10)] 
    public List<AudioClip> FlyDeath;
    [Space(10)] 
    public List<AudioClip> HorfAttack;
    public List<AudioClip> HorfDeath;
    [Space(10)] 
    public List<AudioClip> PooterDeath;
    [Space(10)] 
    public List<AudioClip> WormAttack;
    public List<AudioClip> WormDeath;
    [Space(10)] 
    public List<AudioClip> PacerJump;
    public List<AudioClip> PacerLand;
    public List<AudioClip> PacerDeath;
    [Space(10)] 
    public List<AudioClip> GaperSounds;
    public List<AudioClip> GaperDeath;
    [Space(10)] 
    public List<AudioClip> HiveSounds;
    public List<AudioClip> HiveAttack;
    public List<AudioClip> HiveDeath;
    [Space(10)] 
    public List<AudioClip> MonstroRoar;
    public List<AudioClip> MonstroLand;
    public List<AudioClip> MonstroAttack;
    public List<AudioClip> MonstroDeath;
    [Space(10)] 
    public AudioClip BookPageTurn;
    public AudioClip Plop;
    public AudioClip PickUp;
    public AudioClip FireChoke;
    public AudioClip ChestLand;
    public AudioClip ChestOpen;
    public AudioClip LockDoors;
    public AudioClip BossLockDoors;
    public AudioClip UnLockDoors;
  }
}