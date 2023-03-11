using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleRailMananger : Singleton<DestructibleRailMananger>
{
    public List<CodeRule> m_CodeRuleList;

    public (DestructibleRail, int) DestructilbeRail(ConnectionCode code)
    {
        if (code.GetBaseConnectionCode().Count == 1)
        {
            for (int i = 0; i < 4; i++)
            {
                if (m_CodeRuleList[0].m_ConnectionCode.GetRotateCode(i)[0] == code.GetBaseConnectionCode()[0])
                {
                    return (m_CodeRuleList[0].m_DestructibleRail, i);
                }                
            }
        }

        for (int i = 0; i < m_CodeRuleList.Count; i++)
        {
            if (m_CodeRuleList[i].m_ConnectionCode.CanTransformTo(code))
            {
                return (m_CodeRuleList[i].m_DestructibleRail, m_CodeRuleList[i].m_ConnectionCode.GetRotateTime(code));
            }
        }
        return (m_CodeRuleList[0].m_DestructibleRail, 0);
    }
}
[System.Serializable]
public class CodeRule
{
    public DestructibleRail m_DestructibleRail;
    public ConnectionCode m_ConnectionCode;
}