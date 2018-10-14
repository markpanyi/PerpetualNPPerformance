using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerpetualNBPerformance
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // 初始化技能等级
            NP20s_AllSkillReset();
            Buff_AllSkillReset();
            enermyHP_Write();
        }

        private Boolean clothIsExchange = true;
        private Boolean hasNPServ = false;
        private Boolean hasBufServ = false;
        private Boolean allowCalcu = false;
        private Boolean biKong = false;
        private Boolean biHai = false;
        private static int[] enermy1st = { 23465, 23301, 23301 };
        private static int[] enermy2nd = { 23465, 24319, 81857 };
        private static int[] enermy3rd = { 98090, 152662 };
        private static double[] npTimes = { 4.5, 6.0, 6.75, 7.125, 7.5 };
        private static double[] damageAccuHit = { 0.06, 0.19, 0.39, 0.65, 1.00 };

        // 写入敌方血量
        private void enermyHP_Write()
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

        // 满芙芙设置（应该都满了吧……）
        private void Fufu1000B_Click(object sender, EventArgs e) => FufuText.Text = "1000";

        // R莫百级5宝310满芙芙（135，大佬的R莫）设置
        private void MdAllMaxB_Click(object sender, EventArgs e)
        {
            LvText.Text = "100";
            FufuText.Text = "1000";
            NPLvText.Text = "5";
            MdSkill1Text.Text = "10";
            MdSkill2Text.Text = "10";
            MdSkill3Text.Text = "10";
        }

        // R莫80级310满芙芙（通常的R莫）设置
        private void Md80MaxB_Click(object sender, EventArgs e)
        {
            LvText.Text = "80";
            FufuText.Text = "1000";
            MdSkill1Text.Text = "10";
            MdSkill2Text.Text = "10";
            MdSkill3Text.Text = "10";
        }

        // 310设置
        private void SkillMax_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "ZGlAllMaxB":
                    foreach (Control item in ZGlSkillP.Controls)
                    {
                        if (item is TextBox) item.Text = "10";
                    }
                    break;
                case "ZGlAllMaxBEx":
                    foreach (Control item in ZGlSkillExP.Controls)
                    {
                        if (item is TextBox) item.Text = "10";
                    }
                    break;
                case "HaiAllMaxB":
                    foreach (Control item in HaiSkillP.Controls)
                    {
                        if (item is TextBox) item.Text = "10";
                    }
                    break;
                case "LaAllMaxB":
                    foreach (Control item in LaSkillP.Controls)
                    {
                        if (item is TextBox) item.Text = "10";
                    }
                    break;
                case "MeiAllMaxB":
                    foreach (Control item in MeiSkillP.Controls)
                    {
                        if (item is TextBox) item.Text = "10";
                    }
                    break;
                case "ShaAllMaxB":
                    foreach (Control item in ShaSkillP.Controls)
                    {
                        if (item is TextBox) item.Text = "10";
                    }
                    break;
                case "MaAllMaxB":
                    foreach (Control item in MaSkillP.Controls)
                    {
                        if (item is TextBox) item.Text = "10";
                    }
                    break;
                case "NPAllMaxB":
                    foreach (Control item in NP20GB.Controls)
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
                case "HaiAllMaxExB":
                    foreach (Control item in HaiSkillExP.Controls)
                    {
                        if (item is TextBox) item.Text = "10";
                    }
                    break;
                case "HuaAllMaxB":
                    foreach (Control item in HuaSkillP.Controls)
                    {
                        if (item is TextBox) item.Text = "10";
                    }
                    break;
                case "YuAllMaxB":
                    foreach (Control item in YuSkillP.Controls)
                    {
                        if (item is TextBox) item.Text = "10";
                    }
                    break;
                case "SanAllMaxB":
                    foreach (Control item in SanSkillP.Controls)
                    {
                        if (item is TextBox) item.Text = "10";
                    }
                    break;
                case "XinAllMaxB":
                    foreach (Control item in XinSkillP.Controls)
                    {
                        if (item is TextBox) item.Text = "10";
                    }
                    break;
                case "FenAllMaxB":
                    foreach (Control item in FenSkillP.Controls)
                    {
                        if (item is TextBox) item.Text = "10";
                    }
                    break;
                case "BuAllMaxB":
                    foreach (Control item in BuffGB.Controls)
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
                    break;
            }
        }

        // 双孔明复选，改变相应控件的可操控性
        private void DoubleZGlChB_CheckedChanged(object sender, EventArgs e)
        {
            if (DoubleZGlChB.Checked)
            {
                biKong = true;
                ZGlAllMaxBEx.Enabled = true;
                ZGlSkillExP.Enabled = true;
                NP20GB.Enabled = false;
                DoubleHaiChB.Enabled = false;
                DoubleHaiChB.Checked = false;
                HaiExRB.Enabled = true;
                HaiSkillExP.Enabled = true;
                NP20s_CheckedReset();
            }
            else
            {
                biKong = false;
                ZGlAllMaxBEx.Enabled = false;
                ZGlSkillExP.Enabled = false;
                NP20GB.Enabled = true;
                HaiExRB.Enabled = false;
                HaiExRB.Checked = false;
                NP20s_AllSkillEnabledReset();
                Buff_AllSkillEnabledReset();
            }
            startCalculating();
        }

        // 御主服装更换
        private void ClothRBs_CheckedChanged(object sender, EventArgs e)
        {
            if (ExchangeRB.Checked)
            {
                clothIsExchange = true;
                DoubleZGlChB.Enabled = true;
                NP20s_AllSkillEnabledReset();
                NP20GB.Enabled = true;
                DoubleHaiChB.Enabled = true;
            }
            if (ChargeRB.Checked)
            {
                clothIsExchange = false;
                DoubleZGlChB.Checked = false;
                DoubleZGlChB.Enabled = false;
                NP20s_CheckedReset();
                NP20GB.Enabled = false;
                DoubleHaiChB.Checked = false;
                DoubleHaiChB.Enabled = false;
            }
            startCalculating();
        }

        // 20NP充能选择
        private void NP20s_CheckedChanged(object sender, EventArgs e)
        {
            HaiSkillP.Enabled = HaiRB.Checked;
            LaSkillP.Enabled = LaRB.Checked;
            MeiSkillP.Enabled = MeiRB.Checked;
            ShaSkillP.Enabled = ShaRB.Checked;
            MaSkillP.Enabled = MaRB.Checked;
            if (HaiRB.Checked)
            {
                DoubleHaiChB.Enabled = true;
            }
            else
            {
                DoubleHaiChB.Enabled = false;
                DoubleHaiChB.Checked = false;
            }
            int ch = 0;
            foreach (RadioButton item in NP20P.Controls)
            {
                ch += Convert.ToInt32(item.Checked);
            }
            hasNPServ = (ch == 1);
            startCalculating();
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

        // NP获取提升Buff选择
        private void Buff_CheckedChanged(object sender, EventArgs e)
        {
            HaiSkillExP.Enabled = HaiExRB.Checked;
            HuaSkillP.Enabled = HuaRB.Checked;
            YuSkillP.Enabled = YuRB.Checked;
            SanSkillP.Enabled = SanRB.Checked;
            XinSkillP.Enabled = XinRB.Checked;
            FenSkillP.Enabled = FenRB.Checked;
            int ch = 0;
            foreach (RadioButton item in BuffP.Controls)
            {
                ch += Convert.ToInt32(item.Checked);
            }
            hasBufServ = (ch == 1);
            startCalculating();
        }

        // 双海妈复选，改变相应控件的可操控性
        private void DoubleHaiChB_CheckedChanged(object sender, EventArgs e)
        {
            if (DoubleHaiChB.Checked)
            {
                biHai = true;
                foreach (RadioButton item in BuffP.Controls)
                {
                    item.Enabled = false;
                }
                HaiExRB.Enabled = true;
                HaiExRB.Checked = true;
                HaiSkillExP.Enabled = true;
            }
            else
            {
                biHai = false;
                foreach (RadioButton item in BuffP.Controls)
                {
                    item.Enabled = true;
                }
                HaiExRB.Enabled = false;
                HaiExRB.Checked = false;
                Buff_AllSkillEnabledReset();
            }
        }

        private void HaiEx_EnabledChange()
        {
            HaiExRB.Enabled = DoubleHaiChB.Checked;
            HaiExRB.Checked = DoubleHaiChB.Checked;
            HaiSkillExP.Enabled = DoubleHaiChB.Checked;
        }

        // Buff重置选择
        private void BuResetSlctnB_Click(object sender, EventArgs e)
        {
            Buff_CheckedReset();
            Buff_AllSkillEnabledReset();
        }

        private void Buff_CheckedReset()
        {
            foreach (RadioButton item in BuffP.Controls)
            {
                item.Checked = false;
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
            foreach (Control item in BuffGB.Controls)
            {
                if (item is Panel)
                {
                    foreach (Control itemT in item.Controls)
                    {
                        if (itemT is TextBox) itemT.Text = "1";
                    }
                }
            }
        }

        private void Buff_AllSkillEnabledReset()
        {
            HaiSkillExP.Enabled = false;
            HuaSkillP.Enabled = true;
            YuSkillP.Enabled = true;
            SanSkillP.Enabled = true;
            XinSkillP.Enabled = true;
            FenSkillP.Enabled = true;
            if (DoubleZGlChB.Checked)
            {
                HaiExRB.Enabled = true;
                HaiSkillExP.Enabled = true;
            }
        }

        // 隐藏无关技能
        private void HideNrskChB_CheckedChanged(object sender, EventArgs e)
        {
            if (HideNrskChB.Checked)
            {
                MdSkill2.Visible = false;
                MdSkill2Text.Visible = false;
                ZGlSkill1.Visible = false;
                ZGlSkill1Text.Visible = false;
                ZGlSkill2.Visible = false;
                ZGlSkill2Text.Visible = false;
                ZGlSkillEx1.Visible = false;
                ZGlSkillEx1Text.Visible = false;
                ZGlSkillEx2.Visible = false;
                ZGlSkillEx2Text.Visible = false;
                HaiSkill2.Visible = false;
                HaiSkill2Text.Visible = false;
                LaSkill2.Visible = false;
                LaSkill2Text.Visible = false;
                MeiSkill2.Visible = false;
                MeiSkill2Text.Visible = false;
                MeiSkill3.Visible = false;
                MeiSkill3Text.Visible = false;
                ShaSkill1.Visible = false;
                ShaSkill1Text.Visible = false;
                ShaSkill2.Visible = false;
                ShaSkill2Text.Visible = false;
                MaSkill1.Visible = false;
                MaSkill1Text.Visible = false;
                MaSkill3.Visible = false;
                MaSkill3Text.Visible = false;
                HaiSkillEx2.Visible = false;
                HaiSkillEx2Text.Visible = false;
                HuaSkill3.Visible = false;
                HuaSkill3Text.Visible = false;
                YuSkill1.Visible = false;
                YuSkill1Text.Visible = false;
                YuSkill2.Visible = false;
                YuSkill2Text.Visible = false;
                SanSkill1.Visible = false;
                SanSkill1Text.Visible = false;
                SanSkill2.Visible = false;
                SanSkill2Text.Visible = false;
                XinSkill2.Visible = false;
                XinSkill2Text.Visible = false;
                XinSkill3.Visible = false;
                XinSkill3Text.Visible = false;
                FenSkill2.Visible = false;
                FenSkill2Text.Visible = false;
                FenSkill3.Visible = false;
                FenSkill3Text.Visible = false;
            }
            else
            {
                MdSkill2.Visible = true;
                MdSkill2Text.Visible = true;
                ZGlSkill1.Visible = true;
                ZGlSkill1Text.Visible = true;
                ZGlSkill2.Visible = true;
                ZGlSkill2Text.Visible = true;
                ZGlSkillEx1.Visible = true;
                ZGlSkillEx1Text.Visible = true;
                ZGlSkillEx2.Visible = true;
                ZGlSkillEx2Text.Visible = true;
                HaiSkill2.Visible = true;
                HaiSkill2Text.Visible = true;
                LaSkill2.Visible = true;
                LaSkill2Text.Visible = true;
                MeiSkill2.Visible = true;
                MeiSkill2Text.Visible = true;
                MeiSkill3.Visible = true;
                MeiSkill3Text.Visible = true;
                ShaSkill1.Visible = true;
                ShaSkill1Text.Visible = true;
                ShaSkill2.Visible = true;
                ShaSkill2Text.Visible = true;
                MaSkill1.Visible = true;
                MaSkill1Text.Visible = true;
                MaSkill3.Visible = true;
                MaSkill3Text.Visible = true;
                HaiSkillEx2.Visible = true;
                HaiSkillEx2Text.Visible = true;
                HuaSkill3.Visible = true;
                HuaSkill3Text.Visible = true;
                YuSkill1.Visible = true;
                YuSkill1Text.Visible = true;
                YuSkill2.Visible = true;
                YuSkill2Text.Visible = true;
                SanSkill1.Visible = true;
                SanSkill1Text.Visible = true;
                SanSkill2.Visible = true;
                SanSkill2Text.Visible = true;
                XinSkill2.Visible = true;
                XinSkill2Text.Visible = true;
                XinSkill3.Visible = true;
                XinSkill3Text.Visible = true;
                FenSkill2.Visible = true;
                FenSkill2Text.Visible = true;
                FenSkill3.Visible = true;
                FenSkill3Text.Visible = true;
            }
        }

        // 永久锁定玛修充能10级
        private void MaSkill2Text_EnabledChanged(object sender, EventArgs e)
        {
            MaSkill2Text.Enabled = false;
        }

        // 文本框更改触发计算
        private void TextBox_Text_Changed(object sender, EventArgs e)
        {
            TextBox tbx = (TextBox)sender;
            if (tbx.Text != "" && tbx.Text != null) startCalculating();
        }

        // 满破勾选触发计算
        private void MLBChB_CheckedChanged(object sender, EventArgs e)
        {
            startCalculating();
        }

        // 主程序
        private void appendNPGain(List<int> array, List<string> arrayT, string name, int skill = 10)
        {
            switch (name)
            {
                case "Md":
                    array.Add((skill < 10) ? (skill - 1) + 20 : 30);
                    arrayT.Add("小莫");
                    break;
                case "Kong":
                    array.Add(50);
                    arrayT.Add("孔明");
                    break;
                case "Hai":
                    array.Add((skill < 10) ? (skill - 1) + 10 : 20);
                    arrayT.Add("海妈");
                    break;
                case "La":
                    array.Add(20);
                    arrayT.Add("拉二");
                    break;
                case "Mei":
                    array.Add(20);
                    arrayT.Add("梅林");
                    break;
                case "Sha":
                    array.Add(20);
                    arrayT.Add("莎比");
                    break;
                case "Ma":
                    array.Add(20);
                    arrayT.Add("玛修");
                    break;
                default:
                    break;
            }
        }

        private void appendAtkUP(List<double> array, List<string> arrayT, string name, int skill)
        {
            switch (name)
            {
                case "Kong":
                    array.Add((skill < 10) ? (skill - 1) * 0.01 + 0.2 : 0.3);
                    arrayT.Add("孔明");
                    break;
                case "La":
                    array.Add((skill < 10) ? (skill - 1) * 0.009 + 0.09 : 0.18);
                    arrayT.Add("拉二");
                    break;
                case "Mei":
                    array.Add((skill < 10) ? (skill - 1) * 0.01 + 0.1 : 0.2);
                    arrayT.Add("梅林");
                    break;
                case "Hua":
                    array.Add((skill < 10) ? (skill - 1) * 0.01 + 0.3 : 0.4);
                    arrayT.Add("花嫁");
                    break;
                default:
                    break;
            }
        }

        private void appendArtsUP(List<double> array, List<string> arrayT, string name, int skill)
        {
            switch (name)
            {
                case "Md":
                    array.Add((skill < 10) ? (skill - 1) * 0.01 + 0.2 : 0.3);
                    arrayT.Add("小莫");
                    break;
                case "Hai":
                    array.Add((skill < 10) ? (skill - 1) * 0.005 + 0.15 : 0.2);
                    arrayT.Add("海妈");
                    break;
                case "Yu":
                    array.Add((skill < 10) ? (skill - 1) * 0.02 + 0.3 : 0.5);
                    arrayT.Add("小玉");
                    break;
                default:
                    break;
            }
        }

        private void appendNPUP(List<double> array, List<string> arrayT, string name, int skill)
        {
            switch (name)
            {
                case "Hua":
                    array.Add((skill < 10) ? (skill - 1) * 0.01 + 0.35 : 0.45);
                    arrayT.Add("花嫁");
                    break;
                case "San":
                    array.Add((skill < 10) ? (skill - 1) * 0.02 + 0.1 : 0.3);
                    arrayT.Add("三藏");
                    break;
                case "Xin":
                    array.Add((skill < 10) ? (skill - 1) * 0.01 + 0.2 : 0.3);
                    arrayT.Add("信长");
                    break;
                case "Fen":
                    array.Add((skill < 10) ? (skill - 1) * 0.02 + 0.1 : 0.3);
                    arrayT.Add("芬恩");
                    break;
                default:
                    break;
            }
        }

        private void startCalculating()
        {
            if (ExchangeRB.Checked)
            {
                if (biKong || biHai)
                {
                    allowCalcu = hasBufServ;
                }
                else
                {
                    allowCalcu = hasBufServ && hasNPServ;
                }
            }
            else if (ChargeRB.Checked)
            {
                allowCalcu = hasBufServ;
            }

            // 先判断是不是可以开始计算了
            if (allowCalcu)
            {
                ResultGB.Enabled = true;

                List<int> npAllL = new List<int>();
                List<double> atkAllL = new List<double>();
                List<double> artsAllL = new List<double>();
                List<double> npUpAllL = new List<double>();
                List<string> npAllLN = new List<string>();
                List<string> atkAllLN = new List<string>();
                List<string> artsAllLN = new List<string>();
                List<string> npUpAllLN = new List<string>();
                int npAll = 0;
                double atkAll = 0;
                double artsAll = 0;
                double npUpAll = 0;
                double specAtk = 0;
                double extraAtt = 0;
                double npGainPerHit = 0;
                double atkTotal = 0;

                // 充能服
                if (clothIsExchange == false) {
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
                appendNPGain(npAllL, npAllLN, "Md", Convert.ToInt32(MdSkill3Text.Text));
                appendArtsUP(artsAllL, artsAllLN, "Md", Convert.ToInt32(MdSkill1Text.Text));

                // 小莫被动
                artsAllL.Add(0.05);
                artsAllLN.Add("小莫被动");

                // 礼装效果
                if (CECoB.SelectedIndex == -1)
                {

                }
                else if (CECoB.Items[CECoB.SelectedIndex].ToString() == "二神三脚")
                {
                    specAtk = MLBChB.Checked ? 2.2 : 2.15;
                    mdatk += Convert.ToInt16(Math.Floor(((2000.0 - 500.0) / 99) * (Convert.ToInt16(CELvText.Text) - 1) + 500));
                }
                else if (CECoB.Items[CECoB.SelectedIndex].ToString() == "为御主加油")
                {
                    artsAllL.Add(MLBChB.Checked ? 0.08 : 0.06);
                    artsAllLN.Add("礼装");
                    mdatk += Convert.ToInt16(Math.Floor(((1000.0 - 250.0) / 99) * (Convert.ToInt16(CELvText.Text) - 1) + 250));
                }

                // 看双孔明是否有
                int skill;
                if (biKong)
                {
                    appendNPGain(npAllL, npAllLN, "Kong");
                    appendAtkUP(atkAllL, atkAllLN, "Kong", Convert.ToInt32(ZGlSkillEx3Text.Text));
                    skill = Convert.ToInt32(ZGlSkillEx3Text.Text);
                    extraAtt += (skill < 10) ? (skill - 1) * 30 + 200 : 500;
                }
                // 孔明
                appendNPGain(npAllL, npAllLN, "Kong");
                appendAtkUP(atkAllL, atkAllLN, "Kong", Convert.ToInt32(ZGlSkill3Text.Text));
                skill = Convert.ToInt32(ZGlSkill3Text.Text);
                extraAtt += (skill < 10) ? (skill - 1) * 30 + 200 : 500;

                // 20NP提供
                StringBuilder np20sup = new StringBuilder();
                foreach (RadioButton item in NP20P.Controls)
                {
                    np20sup.Append(item.Checked ? item.Name : "");
                }
                switch (np20sup.ToString())
                {
                    case "HaiRB":
                        appendNPGain(npAllL, npAllLN, "Hai", Convert.ToInt32(HaiSkill1Text.Text));
                        appendArtsUP(artsAllL, artsAllLN, "Hai", Convert.ToInt32(HaiSkill3Text.Text));
                        break;
                    case "LaRB":
                        appendNPGain(npAllL, npAllLN, "La");
                        appendAtkUP(atkAllL, atkAllLN, "La", Convert.ToInt32(LaSkill1Text.Text));
                        break;
                    case "MeiRB":
                        appendNPGain(npAllL, npAllLN, "Mei");
                        appendAtkUP(atkAllL, atkAllLN, "Mei", Convert.ToInt32(MeiSkill1Text.Text));
                        break;
                    case "ShaRB":
                        appendNPGain(npAllL, npAllLN, "Sha");
                        break;
                    case "MaRB":
                        appendNPGain(npAllL, npAllLN, "Ma");
                        break;
                    default:
                        break;
                }

                // Buff提供
                StringBuilder buffsup = new StringBuilder();
                foreach (RadioButton item in BuffP.Controls)
                {
                    buffsup.Append(item.Checked ? item.Name : "");
                }
                switch (buffsup.ToString())
                {
                    case "HaiExRB":
                        appendNPGain(npAllL, npAllLN, "Hai", Convert.ToInt32(HaiSkillEx1Text.Text));
                        appendArtsUP(artsAllL, artsAllLN, "Hai", Convert.ToInt32(HaiSkillEx3Text.Text));
                        break;
                    case "HuaRB":
                        appendNPUP(npUpAllL, npUpAllLN, "Hua", Convert.ToInt32(HuaSkill1Text.Text));
                        appendAtkUP(atkAllL, atkAllLN, "Hua", Convert.ToInt32(HuaSkill2Text.Text));
                        break;
                    case "YuRB":
                        appendArtsUP(artsAllL, artsAllLN, "Yu", Convert.ToInt32(YuSkill3Text.Text));
                        break;
                    case "SanRB":
                        appendNPUP(npUpAllL, npUpAllLN, "San", Convert.ToInt32(SanSkill3Text.Text));
                        break;
                    case "XinRB":
                        appendNPUP(npUpAllL, npUpAllLN, "Xin", Convert.ToInt32(XinSkill1Text.Text));
                        break;
                    case "FenRB":
                        appendNPUP(npUpAllL, npUpAllLN, "Fen", Convert.ToInt32(FenSkill1Text.Text));
                        break;
                    default:
                        break;
                }

                // 加和所有数据
                foreach (int item in npAllL) npAll += item;
                foreach (float item in atkAllL) atkAll += item;
                foreach (float item in artsAllL) artsAll += item;
                foreach (float item in npUpAllL) npUpAll += item;

                // 计算上述条件下每hit无overkill下的NP回收量
                npGainPerHit = 0.71 * (1 + artsAll) * (1 + npUpAll) * 3 * 1.2;

                // 计算上述条件下宝具总伤害（1/2面）
                atkTotal = mdatk * 0.23 * npTime * (1 + artsAll) * 2 * 0.9 * (1 + atkAll) * (1 + specAtk) * 0.9 + extraAtt;

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
                double atkTotal3;
                if (clothIsExchange)
                {
                    atkTotal3 = mdatk * 0.23 * npTime * (1 + artsAll) * 1.1 * (1 + atkAll + 0.3) * (1 + specAtk) * 0.9 + extraAtt;
                }
                else
                {
                    atkTotal3 = mdatk * 0.23 * npTime * (1 + artsAll) * 1.1 * (1 + atkAll) * (1 + specAtk) * 0.9 + extraAtt;
                }
                enermy3Atk.Add(Convert.ToInt32(Math.Floor(atkTotal3)));
                enermy3Atk.Add(Convert.ToInt32(Math.Floor(atkTotal3 * 2)));
                enermy3AtkMax.Add(Convert.ToInt32(Math.Floor(atkTotal3 / 0.9 * 1.1)));
                enermy3AtkMax.Add(Convert.ToInt32(Math.Floor(atkTotal3 * 2 / 0.9 * 1.1)));

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
                StringBuilder Artsupoutput = new StringBuilder();
                StringBuilder NPupoutput = new StringBuilder();

                Atkoutput.Append("小莫Atk：");
                Atkoutput.Append(mdatk.ToString());
                NPoutput.Append("NP：");
                Atkupoutput.Append("攻击力提升：");
                NPupoutput.Append("NP获得量提升：");

                for (int i = 0; i < npAllL.Count(); i++)
                {
                    NPoutput.Append(npAllLN[i]);
                    NPoutput.Append(npAllL[i].ToString());
                    if (i < npAllL.Count() - 1) NPoutput.Append("+");
                }
                NPoutput.Append("=");
                NPoutput.Append(npAll.ToString());

                for (int i = 0; i < atkAllL.Count(); i++)
                {
                    Atkupoutput.Append(atkAllLN[i]);
                    Atkupoutput.Append(atkAllL[i].ToString());
                    if (i < atkAllL.Count() - 1) Atkupoutput.Append("+");
                }
                Atkupoutput.Append("=");
                Atkupoutput.Append(Convert.ToString(Math.Round(atkAll * 1000) / 1000));

                for (int i = 0; i < artsAllL.Count(); i++)
                {
                    Artsupoutput.Append(artsAllLN[i]);
                    Artsupoutput.Append(artsAllL[i].ToString());
                    if (i < artsAllL.Count() - 1) Artsupoutput.Append("+");
                }
                Artsupoutput.Append("=");
                Artsupoutput.Append(Convert.ToString(Math.Round(artsAll * 1000) / 1000));

                for (int i = 0; i < npUpAllL.Count(); i++)
                {
                    NPupoutput.Append(npUpAllLN[i]);
                    NPupoutput.Append(npUpAllL[i].ToString());
                    if (i < npUpAllL.Count() - 1) NPupoutput.Append("+");
                }
                NPupoutput.Append("=");
                NPupoutput.Append(Convert.ToString(Math.Round(npUpAll * 100) / 100));

                NPOutput.Text = NPoutput.ToString();
                AtkOutput.Text = Atkoutput.ToString();
                AtkUPOutput.Text = Atkupoutput.ToString();
                ArtsUpOutput.Text = Artsupoutput.ToString();
                NPUpOutput.Text = NPupoutput.ToString();
            }
            else
            {
                ResultGB.Enabled = false;
            }
        }

        private void CECoB_SelectedIndexChanged(object sender, EventArgs e)
        {
            startCalculating();
        }
    }
}
