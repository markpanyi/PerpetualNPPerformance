using System;
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

        // 双孔明复选，改变相应控件的可操控性
        private void DoubleZGlChB_CheckedChanged(object sender, EventArgs e)
        {
            if (DoubleZGlChB.Checked)
            {
                ZGlAllMaxBEx.Enabled = true;
                HaiExChB.Enabled = true;
                ZGlSkillExP.Enabled = true;
            }
            else
            {
                ZGlAllMaxBEx.Enabled = false;
                HaiExChB.Checked = false;
                HaiExChB.Enabled = false;
                ZGlSkillExP.Enabled = false;
            }
        }

        // 双孔明下选择是否要海妈，改变相应控件的可操作性
        private void HaiExChB_CheckedChanged(object sender, EventArgs e)
        {
            if (HaiExChB.Checked)
            {
                HaiExSkillP.Enabled = true;
            }
            else
            {
                HaiExSkillP.Enabled = false;
            }
        }

        // 孔明310设置
        private void ZGlAllMaxB1_Click(object sender, EventArgs e)
        {
            ZGlSkill1Text.Text = "10";
            ZGlSkill2Text.Text = "10";
            ZGlSkill3Text.Text = "10";
        }

        // 双孔明时第二个孔明310设置
        private void ZGlAllMaxBEx_Click(object sender, EventArgs e)
        {
            ZGlSkillEx1Text.Text = "10";
            ZGlSkillEx2Text.Text = "10";
            ZGlSkillEx3Text.Text = "10";
        }
        
        // 双孔明时海妈310设置
        private void HaiExAllMaxB_Click(object sender, EventArgs e)
        {
            HaiExSkill1Text.Text = "10";
            HaiExSkill2Text.Text = "10";
            HaiExSkill3Text.Text = "10";
        }

        // 御主服装更换
        private void ClothRBs_CheckedChanged(object sender, EventArgs e)
        {
            if (ExchangeRB.Checked)
            {
                DoubleZGlChB.Enabled = true;
                foreach (Panel item in NP20GB.Controls)
                {
                    item.Enabled = true;
                }
                NP20GB.Enabled = true;
            }

            if (ChargeRB.Checked)
            {
                DoubleZGlChB.Checked = false;
                DoubleZGlChB.Enabled = false;
                foreach (RadioButton item in NP20P.Controls)
                {
                    item.Checked = false;
                }
                NP20GB.Enabled = false;
            }
        }

        // 20NP充能选择
        private void NP20s_CheckedChanged(object sender, EventArgs e)
        {
            HaiSkillP.Enabled = HaiRB.Checked;
            LaSkillP.Enabled = LaRB.Checked;
            MeiSkillP.Enabled = MeiRB.Checked;
            ShaSkillP.Enabled = ShaRB.Checked;
            MaSkillP.Enabled = MaRB.Checked;
        }
    }
}
