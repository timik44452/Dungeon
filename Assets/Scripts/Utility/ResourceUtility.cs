using UnityEngine;

public class ResourceUtility
{
    public static ResourceDatabase resourceDatabase
    {
        get
        {
            if (s_resourceDatabase == null)
            {
                s_resourceDatabase = Resources.Load<ResourceDatabase>("ResourceDatabase");
            }

            return s_resourceDatabase;
        }
    }

    public static SkillDatabase skillDatabase
    {
        get
        {
            if (s_skillDatabase == null)
            {
                s_skillDatabase = Resources.Load<SkillDatabase>("SkillDatabase");
            }

            return s_skillDatabase;
        }
    }

    private static ResourceDatabase s_resourceDatabase;
    private static SkillDatabase s_skillDatabase;

}
