using System.Linq;
using _CodeBase.Units.Monsters.Data;

namespace _CodeBase.Points
{
  public class MonstersSpawnPointsStorage : PointsStorage<MonsterSpawnPoint>
  {
    public MonsterSpawnPoint GetPoint(MonsterType type) => Points.FirstOrDefault(point => point.Type == type);
  }
}