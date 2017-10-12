
public enum UIA
{
    uid,
    skinColorId,
    headSkin,
    bodySkin,
    hp,
    hpMax,
    mood,
    moodMax,
    moodLevel,
    dead_time,
    full,
    fullMax,
    fullDec,
    fullDecSec,
    fullTick,
    hungry_slight,
    hungry_medium,
    hungry_extream,
    energy,
    energyDec,
    energyDecSec,
    energyMax,
    energyTick,
    tired_slight,
    tired_medium,
    tired_extream,
}

public class unitData
{
    //int
    public int hp;
    public int corpseTime;
    public int typeId;
    public int bodyId;
    public int headId;
    public int hpMax;
    public int mood;
    public int moodMax;
    public int moodLevel;
    public int full;
    public int fullMax;
    public int fullDec;
    public int fullDecSec;
    public int fullTick;
    public int hungry_slight;
    public int hungry_medium;
    public int hungry_extream;
    public int energy;
    public int energyDec;
    public int energyDecSec;
    public int energyMax;
    public int energyTick;
    public int tired_slight;
    public int tired_medium;
    public int tired_extream;
    public int mood_level_1;
    public int mood_level_2;
    public int mood_level_3;
    public int mood_level_4;
    public int mood_level_5;

    //float
    public float runSpeed;

    //string
    public string name;
}

public class animalData : unitData
{

}

public class humanData : unitData
{

}

public class animalXML : Singleton<animalXML>
{
    public animalData[] data;

    public void init()
    {
        data = new animalData[256];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = new animalData();
        }
    }

    public animalData get(int v)
    {
        return data[v];
    }
}

public class unitDefault : Singleton<unitDefault>
{
    public unitData data;

    public void init()
    {
        data = new unitData();
    }
}
