using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace PerpetualNBPerformance
{
    abstract class CreateXML
    {
        public static string[] SERVANT_ID = { "ZGl", "ZGlEx", "Hai", "La", "Mei", "Sha", "Ma", "HaiEx", "Hua", "Yu", "San", "Xin", "Fen", "Shan", "Dou", "Lin", "Lily", "Niu", "Bu", "Long", "Ni" };
        public static string[] SERVANT_NAME = { "孔明", "孔明2", "海妈", "拉二", "梅林", "莎比", "玛修", "海妈(2)", "花嫁", "小玉", "三藏", "信长", "芬恩", "贤王", "豆爸", "骑凛", "Lily", "牛若", "布妈", "龙娘", "水泥" };

        public static void CreateMdFndmntlXML()
        {
            StringBuilder FndmntlStB = new StringBuilder();
            FndmntlStB.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n\n");
            FndmntlStB.Append("<fundamental>\n");
            FndmntlStB.Append("<servant name=\"小莫\">\n");
            FndmntlStB.Append("<level>80</level>\n");
            FndmntlStB.Append("<fufu>1000</fufu>\n");
            FndmntlStB.Append("<nplv>1</nplv>\n");
            FndmntlStB.Append("<Skill1>10</Skill1>\n");
            FndmntlStB.Append("<Skill2>10</Skill2>\n");
            FndmntlStB.Append("<Skill3>10</Skill3>\n");
            FndmntlStB.Append("</servant>\n");
            FndmntlStB.Append("<celv>20</celv>\n");
            FndmntlStB.Append("</fundamental>\n");

            if (!Directory.Exists(@".\Data")) Directory.CreateDirectory(@".\Data");
            if (File.Exists(@".\Data\MdFndmntlXML.xml")) File.Delete(@".\Data\MdFndmntlXML.xml");
            using (StreamWriter sw =  File.CreateText(@".\Data\MdFndmntlXML.xml")) sw.Write(FndmntlStB.ToString());
        }

        public static XmlDocument CreateMdFndmntlXML_NOTFILE()
        {
            StringBuilder FndmntlStB = new StringBuilder();
            FndmntlStB.Append("<fundamental>\n");
            FndmntlStB.Append("<servant name=\"小莫\">\n");
            FndmntlStB.Append("<level>80</level>\n");
            FndmntlStB.Append("<fufu>1000</fufu>\n");
            FndmntlStB.Append("<nplv>1</nplv>\n");
            FndmntlStB.Append("<Skill1>10</Skill1>\n");
            FndmntlStB.Append("<Skill2>10</Skill2>\n");
            FndmntlStB.Append("<Skill3>10</Skill3>\n");
            FndmntlStB.Append("</servant>\n");
            FndmntlStB.Append("<celv>20</celv>\n");
            FndmntlStB.Append("</fundamental>\n");

            XmlDocument xmld = new XmlDocument();
            xmld.LoadXml(FndmntlStB.ToString());
            return xmld;
        }

        public static void CreateServSkillXML()
        {
            StringBuilder SkillStB = new StringBuilder();
            SkillStB.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n\n");
            SkillStB.Append("<skill>\n");
            for (int i = 0; i < SERVANT_ID.Length; i++)
            {
                SkillStB.Append("<servant id=\"");
                SkillStB.Append(SERVANT_ID[i]);
                SkillStB.Append("\">\n<name>");
                SkillStB.Append(SERVANT_NAME[i]);
                SkillStB.Append("</name>\n");
                SkillStB.Append("<Skill1>1</Skill1>\n");
                SkillStB.Append("<Skill2>1</Skill2>\n");
                SkillStB.Append("<Skill3>1</Skill3>\n");
                SkillStB.Append("</servant>\n");
            }
            SkillStB.Append("</skill>\n");

            if (!Directory.Exists(@".\Data")) Directory.CreateDirectory(@".\Data");
            if (File.Exists(@".\Data\SkillXML.xml")) File.Delete(@".\Data\SkillXML.xml");
            using (StreamWriter sw =  File.CreateText(@".\Data\SkillXML.xml")) sw.Write(SkillStB.ToString());
        }

        public static XmlDocument CreateServSkillXML_NOTFILE()
        {
            StringBuilder SkillStB = new StringBuilder();
            SkillStB.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n\n");
            SkillStB.Append("<skill>\n");
            for (int i = 0; i < SERVANT_ID.Length; i++)
            {
                SkillStB.Append("<servant id=\"");
                SkillStB.Append(SERVANT_ID[i]);
                SkillStB.Append("\">\n<name>");
                SkillStB.Append(SERVANT_NAME[i]);
                SkillStB.Append("</name>\n");
                SkillStB.Append("<Skill1>1</Skill1>\n");
                SkillStB.Append("<Skill2>1</Skill2>\n");
                SkillStB.Append("<Skill3>1</Skill3>\n");
                SkillStB.Append("</servant>\n");
            }
            SkillStB.Append("</skill>\n");

            XmlDocument xmld = new XmlDocument();
            xmld.LoadXml(SkillStB.ToString());
            return xmld;
        }
    }
}
