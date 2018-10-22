using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace PerpetualNBPerformance
{
    public partial class MainForm : Form
    {
        #region 公共变量
        private XmlDocument FndmtXML = new XmlDocument();
        private XmlDocument SkillXML = new XmlDocument();
        private XmlDocument SkillInfo = new XmlDocument();
        private XmlDocument CraftInfo = new XmlDocument();

        private XmlNode skillinfo;
        private XmlNode craftinfo;

        private List<string> UselessSkills = new List<string>();
        #endregion
        #region 公共布尔值
        private Boolean clothIsExchange = true;
        private Boolean hasNPServ = false;
        private Boolean hasBufServ = false;
        private Boolean allowCalcu = false;
        private Boolean pauseChangeDetect = true;
        private Boolean dataInvalid = false;
        private Boolean unusualCE = false;
        #endregion
        #region 固有数据
        private static int[] enermy1st = { 23465, 23301, 23301 };
        private static int[] enermy2nd = { 23465, 24319, 81857 };
        private static int[] enermy3rd = { 98090, 152662 };
        private static double[] npTimes = { 4.5, 6.0, 6.75, 7.125, 7.5 };
        private static double[] damageAccuHit = { 0.06, 0.19, 0.39, 0.65, 1.00 };
        private Dictionary<string, string> servName = new Dictionary<string, string>();
        #endregion

        public MainForm()
        {
            InitializeComponent();
            // 检查XML文件，不存在时创建新文件；并写入数据
            if (!File.Exists(@".\Data\MdFndmntlXML.xml") && !File.Exists(@".\Data\SkillXML.xml"))
            {
                CreateXML.CreateMdFndmntlXML();
                CreateXML.CreateServSkillXML();
                MessageBox.Show("未发现\\Data\\MdMdFndmntlXML.xml与\\Data\\SkillXML.xml文件，已创建新文件", "缺少必要的XML文件", MessageBoxButtons.OK, MessageBoxIcon.Information);
                WriteFndmtlData(true);
                WriteSkillData(true);
            }
            else if (!File.Exists(@".\Data\MdFndmntlXML.xml"))
            {
                CreateXML.CreateMdFndmntlXML();
                MessageBox.Show("未发现\\Data\\MdMdFndmntlXML.xml文件，已创建新文件", "缺少必要的XML文件", MessageBoxButtons.OK, MessageBoxIcon.Information);
                WriteFndmtlData(true);
                if (!WriteSkillData()) WriteSkillData(true);
            }
            else if (!File.Exists(@".\Data\SkillXML.xml"))
            {
                CreateXML.CreateServSkillXML();
                MessageBox.Show("未发现\\Data\\SkillXML.xml文件，已创建新文件", "缺少必要的XML文件", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (!WriteFndmtlData()) WriteFndmtlData(true);
                WriteSkillData(true);

            }
            else
            {
                if (!WriteFndmtlData()) WriteFndmtlData(true);
                if (!WriteSkillData()) WriteSkillData(true);
            }

            // 检查XML文件，不存在时退出
            if (!File.Exists(@".\Data\SkillInfo.xml") || !File.Exists(@".\Data\CraftEInfo.xml"))
            {
                MessageBox.Show("缺少\\Data\\SkillInfo.xml或\\Data\\CraftEInfo.xml文件，程序无法运行", "缺少必要的XML文件", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            else
            {
                SkillInfo.Load(@".\Data\SkillInfo.xml");
                skillinfo = SkillInfo.DocumentElement;

                CraftInfo.Load(@".\Data\CraftEInfo.xml");
                craftinfo = CraftInfo.DocumentElement;

                // 无关技能List写入
                XmlNodeList servant = skillinfo.SelectNodes("servant");
                foreach (XmlNode node in servant)
                {
                    XmlAttribute attrX = node.Attributes["id"];
                    XmlNodeList ulssskills = node.SelectNodes("*[valid=0]");
                    if (attrX.Value == "Hai")
                    {
                        foreach (XmlNode item in ulssskills)
                        {
                            UselessSkills.Add(string.Format("{0}{1}", attrX.Value, item.Name));
                            UselessSkills.Add(string.Format("{0}Ex{1}", attrX.Value, item.Name));
                        }
                    }
                    else
                    {
                        foreach (XmlNode item in ulssskills) UselessSkills.Add(string.Format("{0}{1}", attrX.Value, item.Name));
                    }
                }

                // 礼装选项写入
                List<string> celist = new List<string>();
                XmlNodeList Celist = craftinfo.SelectNodes("//name");
                foreach (XmlNode item in Celist) celist.Add(item.InnerText);
                CECoB.Items.AddRange(celist.ToArray());
            }

            // servName写入
            for (int i = 0; i < CreateXML.SERVANT_ID.Length; i++) servName.Add(CreateXML.SERVANT_ID[i], CreateXML.SERVANT_NAME[i]);

            // 玛修二技能10写入
            MaSkill2Text.Text = "10";

            // 写入敌方血量数据
            EnermyHP_Write();
        }

        #region 写入
        // 写入技能数据
        private Boolean WriteFndmtlData()
        {
            Boolean confrm = true;
            pauseChangeDetect = true;
            try
            {
                FndmtXML.Load(@".\Data\MdFndmntlXML.xml");
                XmlNode servant = FndmtXML.GetElementsByTagName("servant").Item(0);
                LvText.Text = servant["level"].InnerText;
                FufuText.Text = servant["fufu"].InnerText;
                NPLvText.Text = servant["nplv"].InnerText;
                for (int i = 1; i <= 3; i++)
                {
                    this.Controls.Find(string.Format("MdSkill{0}Text", i), true)[0].Text = servant[string.Format("Skill{0}", i)].InnerText;
                }
                CELvText.Text = FndmtXML.GetElementsByTagName("celv").Item(0).InnerText;
            }
            catch (Exception)
            {
                string txt = MessageBox.Show("基础数据尝试写入失败，是否重新生成XML文件以重新写入？\n“是”：重新生成XML文件并尝试重写；\n“否”：不重新生成XML文件，使用默认参数写入", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning).ToString();
                switch (txt)
                {
                    case "Yes": //Yes
                        CreateXML.CreateMdFndmntlXML();
                        confrm = false;
                        break;
                    case "No": //No
                        confrm = false;
                        break;
                    default:
                        break;
                }
            }
            pauseChangeDetect = false;
            return confrm;
        }
        private Boolean WriteFndmtlData(Boolean bl)
        {
            FndmtXML = CreateXML.CreateMdFndmntlXML_NOTFILE();
            pauseChangeDetect = true;
            XmlNode servant = FndmtXML.GetElementsByTagName("servant").Item(0);
            LvText.Text = servant["level"].InnerText;
            FufuText.Text = servant["fufu"].InnerText;
            NPLvText.Text = servant["nplv"].InnerText;
            for (int i = 1; i <= 3; i++)
            {
                this.Controls.Find(string.Format("MdSkill{0}Text", i), true)[0].Text = servant[string.Format("Skill{0}", i)].InnerText;
            }
            CELvText.Text = FndmtXML.GetElementsByTagName("celv").Item(0).InnerText;
            pauseChangeDetect = false;
            return true;
        }

        private Boolean WriteSkillData()
        {
            Boolean confrm = true;
            pauseChangeDetect = true;
            try
            {
                SkillXML.Load(@".\Data\SkillXML.xml");
                XmlNodeList servant = SkillXML.GetElementsByTagName("servant");
                foreach (XmlNode node in servant)
                {
                    XmlAttribute attrX = node.Attributes["id"];
                    for (int i = 1; i <= 3; i++)
                    {
                        string temp = string.Format("Skill{0}", i);
                        this.Controls.Find(string.Format("{0}{1}Text", attrX.Value, temp), true)[0].Text = node.SelectSingleNode(temp).InnerText;
                    }
                }
            }
            catch (Exception)
            {
                string txt = MessageBox.Show("从者技能数据尝试写入失败，是否重新生成XML文件以重新写入？\n“是”：重新生成XML文件并尝试重写；\n“否”：不重新生成XML文件，使用默认参数写入", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning).ToString();
                switch (txt)
                {
                    case "Yes": //Yes
                        CreateXML.CreateServSkillXML();
                        confrm = false;
                        break;
                    case "No": //No
                        confrm = false;
                        break;
                    default:
                        break;
                }

            }
            pauseChangeDetect = false;
            return confrm;
        }
        private Boolean WriteSkillData(Boolean bl)
        {
            SkillXML = CreateXML.CreateServSkillXML_NOTFILE();
            pauseChangeDetect = true;
            XmlNodeList servant = SkillXML.GetElementsByTagName("servant");
            foreach (XmlNode node in servant)
            {
                XmlAttribute attrX = node.Attributes["id"];
                for (int i = 1; i <= 3; i++)
                {
                    string temp = string.Format("Skill{0}", i);
                    this.Controls.Find(string.Format("{0}{1}Text", attrX.Value, temp), true)[0].Text = node.SelectSingleNode(temp).InnerText;
                }
            }
            pauseChangeDetect = false;
            return true;
        }

        // 写入敌方血量
        private void EnermyHP_Write()
        {
            for (int i = 0; i < enermy1st.Length; i++)
            {
                Enermy1LV.Items[0].SubItems[i + 1].Text = enermy1st[i].ToString();
            }
            for (int i = 0; i < enermy2nd.Length; i++)
            {
                Enermy2LV.Items[0].SubItems[i + 1].Text = enermy2nd[i].ToString();
            }
            for (int i = 0; i < enermy3rd.Length; i++)
            {
                Enermy3LV.Items[0].SubItems[i + 1].Text = enermy3rd[i].ToString();
            }
        }
        #endregion

        #region 一键数据写入
        // 满芙芙设置（应该都满了吧……）
        private void Fufu1000B_Click(object sender, EventArgs e) => FufuText.Text = "1000";

        // R莫百级5宝310满芙芙（135，大佬的R莫）设置
        private void MdAllMaxB_Click(object sender, EventArgs e)
        {
            pauseChangeDetect = true;
            LvText.Text = "100";
            FufuText.Text = "1000";
            NPLvText.Text = "5";
            MdSkill1Text.Text = "10";
            MdSkill2Text.Text = "10";
            MdSkill3Text.Text = "10";
            pauseChangeDetect = false;
            StartCalculating();
        }

        // R莫80级310满芙芙（通常的R莫）设置
        private void Md80MaxB_Click(object sender, EventArgs e)
        {
            pauseChangeDetect = true;
            LvText.Text = "80";
            FufuText.Text = "1000";
            MdSkill1Text.Text = "10";
            MdSkill2Text.Text = "10";
            MdSkill3Text.Text = "10";
            pauseChangeDetect = false;
            StartCalculating();
        }

        // 310设置
        private void SkillMax_Click(object sender, EventArgs e)
        {
            pauseChangeDetect = true;
            Button btn = (Button)sender;
            string btnName = btn.Name;
            StringBuilder panelName = new StringBuilder(btnName.Replace("AllMaxB", ""));
            switch (panelName.ToString())
            {
                case "NP20":
                    panelName.Append("GB");
                    GroupBox temp_1 = (GroupBox)this.Controls.Find(panelName.ToString(), true)[0];
                    foreach (Control item in temp_1.Controls)
                    {
                        if (item is Panel)
                        {
                            foreach (Control itemT in item.Controls)
                            {
                                if (itemT is TextBox) itemT.Text = "10";
                            }
                        }
                    }
                    break;
                case "Bu30":
                case "Bu20":
                    panelName.Append("TP");
                    TabPage temp_2 = (TabPage)this.Controls.Find(panelName.ToString(), true)[0];
                    foreach (Control item in temp_2.Controls)
                    {
                        if (item is Panel)
                        {
                            foreach (Control itemT in item.Controls)
                            {
                                if (itemT is TextBox) itemT.Text = "10";
                            }
                        }
                    }
                    break;
                default:
                    panelName.Append("SkillP");
                    Panel temp = (Panel)this.Controls.Find(panelName.ToString(), true)[0];
                    for (int i = 0; i < temp.Controls.Count; i++)
                    {
                        if (temp.Controls[i] is TextBox) temp.Controls[i].Text = "10";
                    }
                    break;
            }
            pauseChangeDetect = false;
            StartCalculating();
        }
        #endregion

        // 御主服装更换
        private void ClothRBs_CheckedChanged(object sender, EventArgs e)
        {
            if (ExchangeRB.Checked) clothIsExchange = true;
            else if (ChargeRB.Checked) clothIsExchange = false;

            DoubleZGlChB.Enabled = !unusualCE && clothIsExchange;
            DoubleHaiChB.Enabled = !DoubleZGlChB.Checked && clothIsExchange && HaiRB.Checked;
            NP20GB.Enabled = !(!(unusualCE || (clothIsExchange && !DoubleZGlChB.Checked)) || (unusualCE && !NoZGlChB.Checked));
            HaiExRB.Enabled = !unusualCE && (!clothIsExchange || DoubleHaiChB.Checked);
            HaiExChB.Enabled = unusualCE && (!clothIsExchange || DoubleHaiChB.Checked);
            StartCalculating();
        }

        // 礼装更换
        private void CECoB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (craftinfo.SelectSingleNode(string.Format("craftes[name=\"{0}\"]", CECoB.SelectedItem.ToString()))["unusual"].InnerText == "1")
            {
                unusualCE = true;
                ExchangeRB.Checked = true;
                ChargeRB.Checked = false;
            }
            else
            {
                unusualCE = false;
                ChargeRB.Checked = true;
            }

            ClothP.Enabled = !unusualCE;
            DoubleZGlChB.Enabled = !unusualCE && clothIsExchange;
            NoZGlChB.Enabled = unusualCE;
            NP20GB.Enabled = !(!(unusualCE || (clothIsExchange && !DoubleZGlChB.Checked)) || (unusualCE && !NoZGlChB.Checked));
            foreach (Control item in BuffP.Controls)
            {
                if (item is RadioButton && item.Name != "HaiExRB") item.Enabled = !unusualCE && !DoubleHaiChB.Checked;
                else if (item is CheckBox && item.Name != "HaiExChB") item.Enabled = unusualCE;
            }
            foreach (Control item in Bu20TP.Controls) item.Enabled = unusualCE;
            HaiExRB.Enabled = !unusualCE && (!clothIsExchange || DoubleHaiChB.Checked);
            HaiExChB.Enabled = unusualCE && (!clothIsExchange || DoubleHaiChB.Checked);
            HaiExSkillP.Enabled = !clothIsExchange || DoubleHaiChB.Checked;
            HaiExRB.Checked = !unusualCE && DoubleHaiChB.Checked;
            HaiExChB.Checked = unusualCE && DoubleHaiChB.Checked;
            StartCalculating();
        }

        #region 孔明/20NP充能从者相关
        // 双孔明复选，改变相应控件的可操控性
        private void DoubleZGlChB_CheckedChanged(object sender, EventArgs e)
        {
            ZGlExSkillP.Enabled = DoubleZGlChB.Checked;
            ZGlExAllMaxB.Enabled = DoubleZGlChB.Checked;
            NP20GB.Enabled = !(!(unusualCE || (clothIsExchange && !DoubleZGlChB.Checked)) || (unusualCE && !NoZGlChB.Checked));
            DoubleHaiChB.Enabled = !DoubleZGlChB.Checked && clothIsExchange && HaiRB.Checked;
            StartCalculating();
        }

        // 无孔明复选
        private void NoZGlChB_CheckedChanged(object sender, EventArgs e)
        {
            ZGlSkillP.Enabled = !NoZGlChB.Checked;
            NP20GB.Enabled = !(!(unusualCE || (clothIsExchange && !DoubleZGlChB.Checked)) || (unusualCE && !NoZGlChB.Checked));
            StartCalculating();
        }

        // 20NP充能选择
        private void NP20s_CheckedChanged(object sender, EventArgs e)
        {
            int ch = 0;
            foreach (RadioButton item in NP20P.Controls)
            {
                ch += Convert.ToInt32(item.Checked);
            }
            hasNPServ = (ch == 1);
            DoubleHaiChB.Enabled = !DoubleZGlChB.Checked && clothIsExchange && HaiRB.Checked;
            StartCalculating();
        }

        // 20NP重置所有
        private void NPResetAllB_Click(object sender, EventArgs e)
        {
            NP20s_CheckedReset();
            NP20s_AllSkillEnabledReset();
            NP20s_AllSkillReset();
        }

        private void NP20s_AllSkillReset()
        {
            pauseChangeDetect = true;
            foreach (Control item in NP20GB.Controls)
            {
                if (item is Panel)
                {
                    foreach (Control itemT in item.Controls)
                    {
                        if (itemT is TextBox) itemT.Text = "1";
                    }
                }
            }
            MaSkill2Text.Text = "10";
            pauseChangeDetect = false;
        }

        private void NP20s_AllSkillEnabledReset()
        {
            foreach (Control item in NP20GB.Controls)
            {
                if (item is Panel) item.Enabled = true;
            }
        }

        // 20NP重置选择
        private void NPResetSlctnB_Click(object sender, EventArgs e)
        {
            NP20s_CheckedReset();
            NP20s_AllSkillEnabledReset();
        }

        private void NP20s_CheckedReset()
        {
            foreach (RadioButton item in NP20P.Controls)
            {
                item.Checked = false;
            }
        }
        #endregion

        #region Buff从者相关
        // NP获取提升Buff选择
        private void Buffs_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control item in BuffP.Controls)
            {
                if (item is RadioButton && item.Name != "HaiExRB") item.Enabled = !unusualCE && !DoubleHaiChB.Checked;
            }
            uint ch = 0;
            foreach (Control item in BuffP.Controls)
            {
                if (item is RadioButton) ch += Convert.ToUInt32(((RadioButton)item).Checked);
            }
            hasBufServ = (ch == 1);
            StartCalculating();
        }
        private void Bu20s_CheckedChanged(object sender, EventArgs e)
        {
            uint ct = 0;
            foreach (Control item in BuffP.Controls)
            {
                if (item is CheckBox && ((CheckBox)item).Checked) ct += 1;
            }
            foreach (Control item in Bu20P.Controls)
            {
                if (item is CheckBox && ((CheckBox)item).Checked) ct += 1;
            }
            hasBufServ = (ct == 2);
            StartCalculating();
        }

        // 双海妈复选，改变相应控件的可操控性
        private void DoubleHaiChB_CheckedChanged(object sender, EventArgs e)
        {
            HaiExRB.Enabled = !unusualCE && (!clothIsExchange || DoubleHaiChB.Checked);
            HaiExChB.Enabled = unusualCE && (!clothIsExchange || DoubleHaiChB.Checked);
            HaiExSkillP.Enabled = !clothIsExchange || DoubleHaiChB.Checked;
            HaiExRB.Checked = !unusualCE && DoubleHaiChB.Checked;
            HaiExChB.Checked = unusualCE && DoubleHaiChB.Checked;
        }

        // Buff重置选择
        private void BuResetSlctnB_Click(object sender, EventArgs e)
        {
            Buff_CheckedReset();
            Buff_AllSkillEnabledReset();
        }

        private void Buff_CheckedReset()
        {
            foreach (Control item in BuffP.Controls)
            {
                if (item is RadioButton) ((RadioButton)item).Checked = false;
                else if (item is CheckBox) ((CheckBox)item).Checked = false;
            }
        }

        // Buff重置所有
        private void BuResetAllB_Click(object sender, EventArgs e)
        {
            Buff_CheckedReset();
            Buff_AllSkillEnabledReset();
            Buff_AllSkillReset();
            DoubleHaiChB.Checked = false;
        }

        private void Buff_AllSkillReset()
        {
            pauseChangeDetect = true;
            foreach (Control item in Bu30TP.Controls) if (item is Panel) foreach (Control itemT in item.Controls) if (itemT is TextBox) itemT.Text = "1";
            pauseChangeDetect = false;
        }

        private void Buff_AllSkillEnabledReset()
        {
            HaiExSkillP.Enabled = true && DoubleZGlChB.Checked;
            HuaSkillP.Enabled = true;
            YuSkillP.Enabled = true;
            SanSkillP.Enabled = true;
            XinSkillP.Enabled = true;
            FenSkillP.Enabled = true;
            ShanSkillP.Enabled = true;
        }

        // Buff20重置选择
        private void Bu20ResetSlctnB_Click(object sender, EventArgs e)
        {
            Bu20_CheckedReset();
            Bu20_AllSkillEnabledReset();
        }

        private void Bu20_CheckedReset()
        {
            foreach (CheckBox item in Bu20P.Controls) item.Checked = false;
        }

        // Buff重置所有
        private void Bu20ResetAllB_Click(object sender, EventArgs e)
        {
            Bu20_CheckedReset();
            Bu20_AllSkillEnabledReset();
            Bu20_AllSkillReset();
        }

        private void Bu20_AllSkillReset()
        {
            pauseChangeDetect = true;
            foreach (Control item in Bu20TP.Controls) if (item is Panel) foreach (Control itemT in item.Controls) if (itemT is TextBox) itemT.Text = "1";
            pauseChangeDetect = false;
        }

        private void Bu20_AllSkillEnabledReset()
        {
            foreach (Control item in Bu20TP.Controls) if (item is Panel) item.Enabled = true;
        }

        // 永久锁定玛修充能10级
        private void MaSkill2Text_EnabledChanged(object sender, EventArgs e)
        {
            MaSkill2Text.Enabled = false;
        }
        #endregion

        // 隐藏无关技能
        private void HideNrskChB_CheckedChanged(object sender, EventArgs e)
        {
            if (HideNrskChB.Checked)
            {
                foreach (string item in UselessSkills)
                {
                    this.Controls.Find(item, true)[0].Visible = false;
                    this.Controls.Find(string.Format("{0}Text", item), true)[0].Visible = false;
                }
            }
            else
            {
                foreach (string item in UselessSkills)
                {
                    this.Controls.Find(item, true)[0].Visible = true;
                    this.Controls.Find(string.Format("{0}Text", item), true)[0].Visible = true;
                }
            }
        }

        // 文本框失去焦点时判断数据是否有效
        private void TextBox_Leave(object sender, EventArgs e)
        {
            if (!dataInvalid) return;
            Boolean error = false;
            TextBox tbx = (TextBox)sender;
            try
            {
                Convert.ToInt32(tbx.Text);
            }
            catch (FormatException)
            {
                error = true;
            }
            if (tbx.Name.IndexOf("Skill") > -1)
            {
                if (error || !((Convert.ToInt32(tbx.Text) > 0 && Convert.ToInt32(tbx.Text) < 11)))
                {
                    MessageBox.Show("技能数值无效，已重置为1", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbx.Text = "1";
                    return;
                }
            }
            else if (tbx.Name.IndexOf("Fufu") > -1)
            {
                if (error || !((Convert.ToInt32(tbx.Text) > -1 && Convert.ToInt32(tbx.Text) < 2000)))
                {
                    MessageBox.Show("芙芙数值无效，已重置为1000", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbx.Text = "1000";
                    return;
                }
            }
            else if (tbx.Name.IndexOf("NPLv") > -1)
            {
                if (error || !((Convert.ToInt32(tbx.Text) > 0 && Convert.ToInt32(tbx.Text) < 6)))
                {
                    MessageBox.Show("宝具等级数值无效，已重置为1", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbx.Text = "1";
                    return;
                }
            }
            else if (tbx.Name.IndexOf("CELv") > -1)
            {
                if (error || !((Convert.ToInt32(tbx.Text) > 0 && Convert.ToInt32(tbx.Text) < 101)))
                {
                    MessageBox.Show("礼装等级数值无效，已重置为20", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbx.Text = "20";
                    return;
                }
            }
            else if (tbx.Name.IndexOf("Lv") > -1)
            {
                if (error || !((Convert.ToInt32(tbx.Text) > 79 && Convert.ToInt32(tbx.Text) < 101)))
                {
                    MessageBox.Show("小莫等级数值无效（需至少80级），已重置为80", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tbx.Text = "80";
                    return;
                }
            }
        }

        #region 触发计算
        // 文本框更改触发计算
        private void TextBox_Text_Changed(object sender, EventArgs e)
        {
            if (pauseChangeDetect) return;
            TextBox tbx = (TextBox)sender;
            try
            {
                Convert.ToInt32(tbx.Text);
            }
            catch (FormatException)
            {
                dataInvalid = true;
                return;
            }
            if (tbx.Name.IndexOf("Skill") > -1)
            {
                if (!((Convert.ToInt32(tbx.Text) > 0 && Convert.ToInt32(tbx.Text) < 11)))
                {
                    dataInvalid = true;
                    return;
                }
            }
            else if (tbx.Name.IndexOf("Fufu") > -1)
            {
                if (!((Convert.ToInt32(tbx.Text) > -1 && Convert.ToInt32(tbx.Text) < 2000)))
                {
                    dataInvalid = true;
                    return;
                }
            }
            else if (tbx.Name.IndexOf("NPLv") > -1)
            {
                if (!((Convert.ToInt32(tbx.Text) > 0 && Convert.ToInt32(tbx.Text) < 6)))
                {
                    dataInvalid = true;
                    return;
                }
            }
            else if (tbx.Name.IndexOf("CELv") > -1)
            {
                if (!((Convert.ToInt32(tbx.Text) > 0 && Convert.ToInt32(tbx.Text) < 101)))
                {
                    dataInvalid = true;
                    return;
                }
            }
            else if (tbx.Name.IndexOf("Lv") > -1)
            {
                if (!((Convert.ToInt32(tbx.Text) > 79 && Convert.ToInt32(tbx.Text) < 101)))
                {
                    dataInvalid = true;
                    return;
                }
            }
            dataInvalid = false;
            StartCalculating();
        }

        // 满破勾选触发计算
        private void MLBChB_CheckedChanged(object sender, EventArgs e)
        {
            StartCalculating();
        }
        #endregion

        private void RB_ChB_EnabledChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton) ((RadioButton)sender).Checked = false;
            else if (sender is CheckBox) ((CheckBox)sender).Checked = false;
        }

        #region 主程序
        private void StartCalculating()
        {
            if (unusualCE)
            {
                allowCalcu = NoZGlChB.Checked ? hasBufServ && hasNPServ : hasBufServ;
            }
            else
            {
                if (ExchangeRB.Checked) allowCalcu = DoubleZGlChB.Checked ? hasBufServ : hasBufServ && hasNPServ;
                else if (ChargeRB.Checked) allowCalcu = hasBufServ;
            }

            // 先判断是不是可以开始计算了
            if (allowCalcu)
            {
                try
                {
                    XmlNode craft = craftinfo.SelectSingleNode(string.Format("craftes[name='{0}']", CECoB.SelectedItem.ToString()));
                }
                catch (Exception)
                {
                    MessageBox.Show("礼装无效，请重新选择礼装！", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ResultGB.Enabled = true;

                List<int> npAllL = new List<int>();
                List<int> extraAtkAllL = new List<int>();
                List<double> npdmgUpAllL = new List<double>();
                List<double> atkUpAllL = new List<double>();
                List<double> artsUpAllL = new List<double>();
                List<double> npUpAllL = new List<double>();
                List<string> npAllLN = new List<string>();
                List<string> extraAtkAllLN = new List<string>();
                List<string> npdmgUpAllLN = new List<string>();
                List<string> atkUpAllLN = new List<string>();
                List<string> artsUpAllLN = new List<string>();
                List<string> npUpAllLN = new List<string>();

                int npAll = 0;
                int extraAtkAll = 0;
                double atkAll = 0;
                double artsAll = 0;
                double npUpAll = 0;
                double npdmgAll = 0;
                double npGainPerHit = 0;
                double atkTotal = 0;
                double defendDown = 0;

                // 充能服
                if (clothIsExchange == false)
                {
                    npAllL.Add(20);
                    npAllLN.Add("充能服");
                }

                // 小莫数据
                int mdatk = Convert.ToInt16(Math.Floor((9212.0 - 1535.0) * (1 + 0.1265 * (Convert.ToInt16(LvText.Text) - 80) / 10) + 1535));
                int mdnplv = Convert.ToInt16(NPLvText.Text);
                double npTime = npTimes[Convert.ToInt32(NPLvText.Text) - 1];

                // 芙芙数据
                mdatk += Convert.ToInt16(FufuText.Text);

                // 小莫主动
                npAllL.Add((Convert.ToInt32(MdSkill3Text.Text) < 10) ? (Convert.ToInt32(MdSkill3Text.Text) - 1) + 20 : 30);
                npAllLN.Add("小莫");
                artsUpAllL.Add((Convert.ToInt32(MdSkill1Text.Text) < 10) ? (Convert.ToInt32(MdSkill1Text.Text) - 1) * 0.01 + 0.2 : 0.3);
                artsUpAllLN.Add("小莫");

                // 小莫被动
                artsUpAllL.Add(0.05);
                artsUpAllLN.Add("小莫被动");

                // 礼装效果
                XmlNode crafte = craftinfo.SelectSingleNode(string.Format("craftes[name='{0}']", CECoB.SelectedItem.ToString()));
                int atk_1 = Convert.ToInt32(crafte["atk_1"].InnerText);
                int atk_C = Convert.ToInt32(crafte["atk_C"].InnerText);
                mdatk += Convert.ToInt16(Math.Floor(((atk_C - atk_1) / 99.0) * (Convert.ToInt32(CELvText.Text) - 1))) + atk_1;
                foreach (XmlNode eff in crafte.SelectNodes("eff"))
                {
                    switch (eff.Attributes["type"].Value)
                    {
                        case "np":
                            npAllL.Add(MLBChB.Checked ? Convert.ToInt32(eff["eff_M"].InnerText) : Convert.ToInt32(eff["eff_N"].InnerText));
                            npAllLN.Add(MLBChB.Checked ? "满破礼装" : "礼装");
                            break;
                        case "atkUp":
                            atkUpAllL.Add(MLBChB.Checked ? Convert.ToDouble(eff["eff_M"].InnerText) : Convert.ToDouble(eff["eff_N"].InnerText));
                            atkUpAllLN.Add(MLBChB.Checked ? "满破礼装" : "礼装");
                            break;
                        case "artsUp":
                            artsUpAllL.Add(MLBChB.Checked ? Convert.ToDouble(eff["eff_M"].InnerText) : Convert.ToDouble(eff["eff_N"].InnerText));
                            artsUpAllLN.Add(MLBChB.Checked ? "满破礼装" : "礼装");
                            break;
                        case "npUp":
                            npUpAllL.Add(MLBChB.Checked ? Convert.ToDouble(eff["eff_M"].InnerText) : Convert.ToDouble(eff["eff_N"].InnerText));
                            npUpAllLN.Add(MLBChB.Checked ? "满破礼装" : "礼装");
                            break;
                        case "npdmgUp":
                            npdmgUpAllL.Add(MLBChB.Checked ? Convert.ToDouble(eff["eff_M"].InnerText) : Convert.ToDouble(eff["eff_N"].InnerText));
                            npdmgUpAllLN.Add(MLBChB.Checked ? "满破礼装" : "礼装");
                            break;
                    }
                }

                // 孔明
                if (!NoZGlChB.Checked)
                {
                    int skllLv, zglnpeff = 0, zglextraatk = 0;
                    double npeff1, npeffX, atkeff1 = 0, atkeffX = 0, extraatk1 = 0, extraatkX = 0, zglatkupeff = 0;
                    XmlNode zglservant = skillinfo.SelectSingleNode("servant[@id=\"ZGl\"]");
                    XmlNodeList zglvalSkills = zglservant.SelectNodes("*[valid=1]");
                    foreach (XmlNode node in zglvalSkills)
                    {
                        skllLv = Convert.ToInt32(this.Controls.Find(string.Format("ZGl{0}Text", node.Name), true)[0].Text);
                        foreach (XmlNode eff in node.SelectNodes("eff"))
                        {
                            switch (eff.Attributes["type"].Value)
                            {
                                case "np":
                                    npeff1 = Convert.ToInt32(eff["eff_num1"].InnerText);
                                    npeffX = Convert.ToInt32(eff["eff_numX"].InnerText);
                                    zglnpeff += Convert.ToInt32(skllLv == 10 ? npeffX : npeff1 + (npeffX - npeff1) / 10 * (skllLv - 1));
                                    break;
                                case "atkUp":
                                    atkeff1 = Convert.ToDouble(eff["eff_num1"].InnerText);
                                    atkeffX = Convert.ToDouble(eff["eff_numX"].InnerText);
                                    zglatkupeff = skllLv == 10 ? atkeffX : atkeff1 + (atkeffX - atkeff1) / 10 * (skllLv - 1);
                                    break;
                                case "extraAtk":
                                    extraatk1 = Convert.ToDouble(eff["eff_num1"].InnerText);
                                    extraatkX = Convert.ToDouble(eff["eff_numX"].InnerText);
                                    zglextraatk = Convert.ToInt32(skllLv == 10 ? extraatkX : extraatk1 + (extraatkX - extraatk1) / 10 * (skllLv - 1));
                                    break;
                            }
                        }
                    }
                    npAllL.Add(zglnpeff);
                    npAllLN.Add(servName["ZGl"]);
                    atkUpAllL.Add(zglatkupeff);
                    atkUpAllLN.Add(servName["ZGl"]);
                    extraAtkAllL.Add(zglextraatk);
                    extraAtkAllLN.Add(servName["ZGl"]);
                    if (DoubleZGlChB.Checked)
                    {
                        skllLv = Convert.ToInt32(this.Controls.Find("ZGlExSkill3Text", true)[0].Text);
                        zglatkupeff = skllLv == 10 ? atkeffX : atkeff1 + (atkeffX - atkeff1) / 10 * (skllLv - 1);
                        zglextraatk = Convert.ToInt32(skllLv == 10 ? extraatkX : extraatk1 + (extraatkX - extraatk1) / 10 * (skllLv - 1));
                        npAllL.Add(zglnpeff);
                        npAllLN.Add(servName["ZGl"]);
                        atkUpAllL.Add(zglatkupeff);
                        atkUpAllLN.Add(servName["ZGl"]);
                        extraAtkAllL.Add(zglextraatk);
                        extraAtkAllLN.Add(servName["ZGl"]);
                    }
                }

                // 20NP提供
                StringBuilder np20sup = new StringBuilder();
                foreach (RadioButton item in NP20P.Controls)
                {
                    np20sup.Append(item.Checked ? item.Name : "");
                }
                np20sup.Replace("RB", "");
                if (np20sup.ToString() != "")
                {
                    XmlNode npservant = skillinfo.SelectSingleNode(string.Format("servant[@id=\"{0}\"]", np20sup.ToString()));
                    XmlNodeList npvalSkills = npservant.SelectNodes("*[valid=1]");
                    foreach (XmlNode node in npvalSkills)
                    {
                        int skllLv = Convert.ToInt32(this.Controls.Find(string.Format("{0}{1}Text", np20sup.ToString(), node.Name), true)[0].Text);
                        foreach (XmlNode eff in node.SelectNodes("eff"))
                        {
                            double eff1, effX, effect;
                            switch (eff.Attributes["type"].Value)
                            {
                                case "np":
                                    eff1 = Convert.ToInt32(eff["eff_num1"].InnerText);
                                    effX = Convert.ToInt32(eff["eff_numX"].InnerText);
                                    effect = skllLv == 10 ? effX : eff1 + (effX - eff1) / 10 * (skllLv - 1);
                                    npAllL.Add(Convert.ToInt32(effect));
                                    npAllLN.Add(servName[np20sup.ToString()]);
                                    break;
                                case "atkUp":
                                    eff1 = Convert.ToDouble(eff["eff_num1"].InnerText);
                                    effX = Convert.ToDouble(eff["eff_numX"].InnerText);
                                    effect = skllLv == 10 ? effX : eff1 + (effX - eff1) / 10 * (skllLv - 1);
                                    atkUpAllL.Add(effect);
                                    atkUpAllLN.Add(servName[np20sup.ToString()]);
                                    break;
                                case "artsUp":
                                    eff1 = Convert.ToDouble(eff["eff_num1"].InnerText);
                                    effX = Convert.ToDouble(eff["eff_numX"].InnerText);
                                    effect = skllLv == 10 ? effX : eff1 + (effX - eff1) / 10 * (skllLv - 1);
                                    artsUpAllL.Add(effect);
                                    artsUpAllLN.Add(servName[np20sup.ToString()]);
                                    break;
                                case "npUp":
                                    eff1 = Convert.ToDouble(eff["eff_num1"].InnerText);
                                    effX = Convert.ToDouble(eff["eff_numX"].InnerText);
                                    effect = skllLv == 10 ? effX : eff1 + (effX - eff1) / 10 * (skllLv - 1);
                                    npUpAllL.Add(effect);
                                    npUpAllLN.Add(servName[np20sup.ToString()]);
                                    break;
                            }
                        }
                    }
                }

                // Buff提供
                if (unusualCE)
                {
                    List<string> buffssup = new List<string>();
                    foreach (Control item in BuffP.Controls)
                    {
                        if (item is CheckBox && ((CheckBox)item).Checked) buffssup.Add(item.Name.Replace("ChB", "").Replace("Ex", ""));
                    }
                    foreach (Control item in Bu20P.Controls)
                    {
                        if (item is CheckBox && ((CheckBox)item).Checked) buffssup.Add(item.Name.Replace("ChB", ""));
                    }
                    foreach (string buffsup in buffssup)
                    {
                        XmlNode buservant = skillinfo.SelectSingleNode(string.Format("servant[@id=\"{0}\"]", buffsup));
                        XmlNodeList buvalSkills = buservant.SelectNodes("*[valid=1]");
                        foreach (XmlNode node in buvalSkills)
                        {
                            int skllLv = Convert.ToInt32(this.Controls.Find(string.Format("{0}{1}Text", buffsup.ToString(), node.Name), true)[0].Text);
                            foreach (XmlNode eff in node.SelectNodes("eff"))
                            {
                                double eff1, effX, effect;
                                switch (eff.Attributes["type"].Value)
                                {
                                    case "np":
                                        eff1 = Convert.ToInt32(eff.SelectSingleNode("eff_num1").InnerText);
                                        effX = Convert.ToInt32(eff.SelectSingleNode("eff_numX").InnerText);
                                        effect = skllLv == 10 ? effX : eff1 + (effX - eff1) / 10 * (skllLv - 1);
                                        npAllL.Add(Convert.ToInt32(effect));
                                        npAllLN.Add(servName[buffsup.ToString()]);
                                        break;
                                    case "atkUp":
                                        eff1 = Convert.ToDouble(eff.SelectSingleNode("eff_num1").InnerText);
                                        effX = Convert.ToDouble(eff.SelectSingleNode("eff_numX").InnerText);
                                        effect = skllLv == 10 ? effX : eff1 + (effX - eff1) / 10 * (skllLv - 1);
                                        atkUpAllL.Add(effect);
                                        atkUpAllLN.Add(servName[buffsup.ToString()]);
                                        break;
                                    case "artsUp":
                                        eff1 = Convert.ToDouble(eff.SelectSingleNode("eff_num1").InnerText);
                                        effX = Convert.ToDouble(eff.SelectSingleNode("eff_numX").InnerText);
                                        effect = skllLv == 10 ? effX : eff1 + (effX - eff1) / 10 * (skllLv - 1);
                                        artsUpAllL.Add(effect);
                                        artsUpAllLN.Add(servName[buffsup.ToString()]);
                                        break;
                                    case "npUp":
                                        eff1 = Convert.ToDouble(eff.SelectSingleNode("eff_num1").InnerText);
                                        effX = Convert.ToDouble(eff.SelectSingleNode("eff_numX").InnerText);
                                        effect = skllLv == 10 ? effX : eff1 + (effX - eff1) / 10 * (skllLv - 1);
                                        npUpAllL.Add(effect);
                                        npUpAllLN.Add(servName[buffsup.ToString()]);
                                        break;
                                    case "defendDown":
                                        eff1 = Convert.ToDouble(eff.SelectSingleNode("eff_num1").InnerText);
                                        effX = Convert.ToDouble(eff.SelectSingleNode("eff_numX").InnerText);
                                        defendDown = skllLv == 10 ? effX : eff1 + (effX - eff1) / 10 * (skllLv - 1);
                                        break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    StringBuilder buffsup = new StringBuilder();
                    foreach (Control item in BuffP.Controls)
                    {
                        if (item is RadioButton) buffsup.Append(((RadioButton)item).Checked ? item.Name : "");
                    }
                    buffsup.Replace("RB", "");
                    buffsup.Replace("Ex", "");
                    XmlNode buservant = skillinfo.SelectSingleNode(string.Format("servant[@id=\"{0}\"]", buffsup.ToString()));
                    XmlNodeList buvalSkills = buservant.SelectNodes("*[valid=1]");
                    foreach (XmlNode node in buvalSkills)
                    {
                        int skllLv = Convert.ToInt32(this.Controls.Find(string.Format("{0}{1}Text", buffsup.ToString(), node.Name), true)[0].Text);
                        foreach (XmlNode eff in node.SelectNodes("eff"))
                        {
                            double eff1, effX, effect;
                            switch (eff.Attributes["type"].Value)
                            {
                                case "np":
                                    eff1 = Convert.ToInt32(eff.SelectSingleNode("eff_num1").InnerText);
                                    effX = Convert.ToInt32(eff.SelectSingleNode("eff_numX").InnerText);
                                    effect = skllLv == 10 ? effX : eff1 + (effX - eff1) / 10 * (skllLv - 1);
                                    npAllL.Add(Convert.ToInt32(effect));
                                    npAllLN.Add(servName[buffsup.ToString()]);
                                    break;
                                case "atkUp":
                                    eff1 = Convert.ToDouble(eff.SelectSingleNode("eff_num1").InnerText);
                                    effX = Convert.ToDouble(eff.SelectSingleNode("eff_numX").InnerText);
                                    effect = skllLv == 10 ? effX : eff1 + (effX - eff1) / 10 * (skllLv - 1);
                                    atkUpAllL.Add(effect);
                                    atkUpAllLN.Add(servName[buffsup.ToString()]);
                                    break;
                                case "artsUp":
                                    eff1 = Convert.ToDouble(eff.SelectSingleNode("eff_num1").InnerText);
                                    effX = Convert.ToDouble(eff.SelectSingleNode("eff_numX").InnerText);
                                    effect = skllLv == 10 ? effX : eff1 + (effX - eff1) / 10 * (skllLv - 1);
                                    artsUpAllL.Add(effect);
                                    artsUpAllLN.Add(servName[buffsup.ToString()]);
                                    break;
                                case "npUp":
                                    eff1 = Convert.ToDouble(eff.SelectSingleNode("eff_num1").InnerText);
                                    effX = Convert.ToDouble(eff.SelectSingleNode("eff_numX").InnerText);
                                    effect = skllLv == 10 ? effX : eff1 + (effX - eff1) / 10 * (skllLv - 1);
                                    npUpAllL.Add(effect);
                                    npUpAllLN.Add(servName[buffsup.ToString()]);
                                    break;
                            }
                        }
                    }
                }

                // 加和所有数据
                foreach (int item in npAllL) npAll += item;
                foreach (int item in extraAtkAllL) extraAtkAll += item;
                foreach (float item in atkUpAllL) atkAll += item;
                foreach (float item in artsUpAllL) artsAll += item;
                foreach (float item in npUpAllL) npUpAll += item;
                foreach (float item in npdmgUpAllL) npdmgAll += item;

                // 计算上述条件下每hit无overkill下的NP回收量
                npGainPerHit = 0.71 * (1 + artsAll) * (1 + npUpAll) * 3 * 1.2;

                // 计算上述条件下宝具总伤害（1/2面）
                atkTotal = mdatk * 0.23 * npTime * (1 + artsAll) * 2 * 0.9 * (1 + atkAll) * (1 + npdmgAll) * 0.9 + extraAtkAll;

                // 计算总np
                List<double> enermy1NP = new List<double>();
                List<int> enermy1Atk = new List<int>();
                List<double> enermy2NP = new List<double>();
                List<int> enermy2Atk = new List<int>();
                List<int> enermy3Atk = new List<int>();
                List<int> enermy3AtkMax = new List<int>();
                foreach (int item in enermy1st)
                {
                    double i = 0;
                    foreach (double items in damageAccuHit) i += ((items * atkTotal >= item) ? (npGainPerHit * 1.5) : npGainPerHit);
                    enermy1NP.Add(i);
                }
                foreach (int item in enermy2nd)
                {
                    double i = 0;
                    foreach (double items in damageAccuHit) i += ((items * atkTotal >= item) ? (npGainPerHit * 1.5) : npGainPerHit);
                    enermy2NP.Add(i);
                }

                // 重写入数组
                double enermy1Totalnp = 0;
                double enermy2Totalnp = 0;
                for (int i = 0; i < enermy1NP.Count(); i++)
                {
                    enermy1Totalnp += enermy1NP[i];
                }
                for (int i = 0; i < enermy2NP.Count(); i++)
                {
                    enermy2Totalnp += enermy2NP[i];
                }
                enermy1NP.Add(enermy1Totalnp);
                enermy2NP.Add(enermy2Totalnp);
                for (int i = 0; i < 3; i++) enermy1Atk.Add(Convert.ToInt32(Math.Floor(atkTotal)));
                for (int i = 0; i < 3; i++) enermy2Atk.Add(Convert.ToInt32(Math.Floor(atkTotal)));
                double atkTotal31, atkTotal32;
                if (clothIsExchange)
                {
                    atkTotal31 = mdatk * 0.23 * npTime * (1 + artsAll) * 1.1 * (1 + atkAll + defendDown + 0.3) * (1 + npdmgAll) * 0.9;
                    atkTotal32 = mdatk * 0.23 * npTime * (1 + artsAll) * 1.1 * (1 + atkAll + 0.3) * (1 + npdmgAll) * 0.9 * 2;
                }
                else
                {
                    atkTotal31 = mdatk * 0.23 * npTime * (1 + artsAll) * 1.1 * (1 + atkAll + defendDown) * (1 + npdmgAll) * 0.9;
                    atkTotal32 = mdatk * 0.23 * npTime * (1 + artsAll) * 1.1 * (1 + atkAll) * (1 + npdmgAll) * 0.9 * 2;
                }
                enermy3Atk.Add(Convert.ToInt32(Math.Floor(atkTotal31) + extraAtkAll));
                enermy3Atk.Add(Convert.ToInt32(Math.Floor(atkTotal32) + extraAtkAll));
                enermy3AtkMax.Add(Convert.ToInt32(Math.Floor(atkTotal31 / 0.9 * 1.1) + extraAtkAll));
                enermy3AtkMax.Add(Convert.ToInt32(Math.Floor(atkTotal32 / 0.9 * 1.1) + extraAtkAll));

                // 写入结果
                for (int i = 0; i < enermy1Atk.Count(); i++)
                {
                    Enermy1LV.Items[1].SubItems[i + 1].Text = enermy1Atk[i].ToString();
                }
                for (int i = 0; i < enermy2Atk.Count(); i++)
                {
                    Enermy2LV.Items[1].SubItems[i + 1].Text = enermy2Atk[i].ToString();
                }
                for (int i = 0; i < enermy3Atk.Count(); i++)
                {
                    Enermy3LV.Items[1].SubItems[i + 1].Text = enermy3Atk[i].ToString();
                }
                for (int i = 0; i < enermy3AtkMax.Count(); i++)
                {
                    Enermy3LV.Items[2].SubItems[i + 1].Text = enermy3AtkMax[i].ToString();
                }
                for (int i = 0; i < enermy1NP.Count(); i++)
                {
                    Enermy1LV.Items[2].SubItems[i + 1].Text = Convert.ToString(Math.Floor(enermy1NP[i] * 10) / 10);
                }
                for (int i = 0; i < enermy2NP.Count(); i++)
                {
                    Enermy2LV.Items[2].SubItems[i + 1].Text = Convert.ToString(Math.Floor(enermy2NP[i] * 10) / 10);
                }
                if (Convert.ToDouble(Enermy1LV.Items[2].SubItems[4].Text) < 99.0)
                {
                    Enermy1LV.Items[2].SubItems[4].ForeColor = Color.Red;
                }

                StringBuilder NPoutput = new StringBuilder();
                StringBuilder Atkoutput = new StringBuilder();
                StringBuilder Atkupoutput = new StringBuilder();
                StringBuilder Npdmgupoutput = new StringBuilder();
                StringBuilder Artsupoutput = new StringBuilder();
                StringBuilder NPupoutput = new StringBuilder();
                StringBuilder ExtraAtkoutput = new StringBuilder();

                Atkoutput.Append("小莫Atk：");
                Atkoutput.Append(mdatk.ToString());
                NPoutput.Append("NP：");
                Atkupoutput.Append("攻击力提升：");
                Npdmgupoutput.Append("宝具威力提升：");
                NPupoutput.Append("NP获得量提升：");
                ExtraAtkoutput.Append("额外伤害增加：");

                for (int i = 0; i < npAllL.Count(); i++)
                {
                    NPoutput.Append(npAllLN[i]);
                    NPoutput.Append(npAllL[i].ToString());
                    if (i < npAllL.Count() - 1) NPoutput.Append("+");
                    else NPoutput.Append("=");
                }
                NPoutput.Append(npAll.ToString());

                for (int i = 0; i < atkUpAllL.Count(); i++)
                {
                    Atkupoutput.Append(atkUpAllLN[i]);
                    Atkupoutput.Append((atkUpAllL[i] * 100).ToString());
                    Atkupoutput.Append("%");
                    if (i < atkUpAllL.Count() - 1) Atkupoutput.Append("+");
                    else Atkupoutput.Append("=");
                }
                Atkupoutput.Append(Convert.ToString(Math.Round(atkAll * 10000) / 100));
                Atkupoutput.Append("%");

                for (int i = 0; i < npdmgUpAllL.Count(); i++)
                {
                    Npdmgupoutput.Append(npdmgUpAllLN[i]);
                    Npdmgupoutput.Append((npdmgUpAllL[i] * 100).ToString());
                    Npdmgupoutput.Append("%");
                    if (i < npdmgUpAllL.Count() - 1) Npdmgupoutput.Append("+");
                    else Npdmgupoutput.Append("=");
                }
                Npdmgupoutput.Append(Convert.ToString(Math.Round(npdmgAll * 100)));
                Npdmgupoutput.Append("%");

                for (int i = 0; i < artsUpAllL.Count(); i++)
                {
                    Artsupoutput.Append(artsUpAllLN[i]);
                    Artsupoutput.Append((artsUpAllL[i] * 100).ToString());
                    Artsupoutput.Append("%");
                    if (i < artsUpAllL.Count() - 1) Artsupoutput.Append("+");
                    else Artsupoutput.Append("=");
                }
                Artsupoutput.Append(Convert.ToString(Math.Round(artsAll * 1000) / 10));
                Artsupoutput.Append("%");

                for (int i = 0; i < npUpAllL.Count(); i++)
                {
                    NPupoutput.Append(npUpAllLN[i]);
                    NPupoutput.Append((npUpAllL[i] * 100).ToString());
                    NPupoutput.Append("%");
                    if (i < npUpAllL.Count() - 1) NPupoutput.Append("+");
                    else NPupoutput.Append("=");
                }
                NPupoutput.Append(Convert.ToString(Math.Round(npUpAll * 100)));
                NPupoutput.Append("%");

                for (int i = 0; i < extraAtkAllL.Count(); i++)
                {
                    ExtraAtkoutput.Append(extraAtkAllLN[i]);
                    ExtraAtkoutput.Append(extraAtkAllL[i].ToString());
                    if (i < extraAtkAllL.Count() - 1) ExtraAtkoutput.Append("+");
                    else ExtraAtkoutput.Append("=");
                }
                ExtraAtkoutput.Append(extraAtkAll.ToString());

                NPOutput.Text = NPoutput.ToString();
                AtkOutput.Text = Atkoutput.ToString();
                AtkUPOutput.Text = Atkupoutput.ToString();
                NpDmgUpOutput.Text = Npdmgupoutput.ToString();
                ArtsUpOutput.Text = Artsupoutput.ToString();
                NPUpOutput.Text = NPupoutput.ToString();
                ExtraAtkOutput.Text = ExtraAtkoutput.ToString();
            }
            else
            {
                ResultGB.Enabled = false;
            }
        }

        #endregion

        #region  保存
        private void SaveFndmtlB_Click(object sender, EventArgs e)
        {
            XmlDocument XML = CreateXML.CreateMdFndmntlXML_NOTFILE();
            XmlNode servant = XML.GetElementsByTagName("servant").Item(0);
            servant["level"].InnerText = LvText.Text;
            servant["fufu"].InnerText = FufuText.Text;
            servant["nplv"].InnerText = NPLvText.Text;
            for (int i = 1; i <= 3; i++)
            {
                servant[string.Format("Skill{0}", i)].InnerText = this.Controls.Find(string.Format("MdSkill{0}Text", i), true)[0].Text;
            }
            FndmtXML.GetElementsByTagName("celv").Item(0).InnerText = CELvText.Text;
            XML.Save(@".\Data\MdFndmntlXML.xml");
            MessageBox.Show("保存成功！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveSkillB_Click(object sender, EventArgs e)
        {
            XmlDocument XML = CreateXML.CreateServSkillXML_NOTFILE();
            XmlNodeList servant = XML.GetElementsByTagName("servant");
            foreach (XmlNode node in servant)
            {
                if (node.HasChildNodes)
                {
                    XmlAttribute attrX = node.Attributes["id"];
                    for (int i = 1; i <= 3; i++)
                    {
                        string temp = string.Format("Skill{0}", i);
                        node.SelectSingleNode(temp).InnerText = this.Controls.Find(string.Format("{0}{1}Text", attrX.Value, temp), true)[0].Text;
                    }
                }
            }
            XML.Save(@".\Data\SkillXML.xml");
            MessageBox.Show("保存成功！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ReLoadB_Click(object sender, EventArgs e)
        {
            if (!WriteFndmtlData()) WriteFndmtlData(true);
            if (!WriteSkillData()) WriteSkillData(true);
            MaSkill2Text.Text = "10";
            StartCalculating();
        }
        #endregion
    }
}
