using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace ppobkki
{
    public partial class Mainform : Form
    {
        private string sWinName = "";

        public Mainform()
        {
            InitializeComponent();

        }

        /// <summary>
        /// 텍스트 박스에 입력이 있으몇 추가, 없으면 파일불러오기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tbAdd.Text.Trim().Length > 0)
            {
                string sTrim = tbAdd.Text.Trim();
                if (lbUserList.Items.IndexOf(sTrim) > 0)
                {
                    MessageBox.Show("이미 존재합니다.", "대상 추가", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                lbUserList.Items.Add(tbAdd.Text.Trim());
                tbAdd.Text = "";
            }
            else
            { 
                //파일 불러오기
                //엔터값으로 구분
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "Text File(*.txt)|*.txt";
                openFile.InitialDirectory = @"c:\";
                openFile.Title = "사용자 목록파일을 선택해 주십시오.";

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    //FileStream listFile = new FileStream(openFile.SafeFileName, FileMode.Open, FileAccess.Read);
                    
                    //while (string sr = listFile.re
                    
                    using (StreamReader sr = new StreamReader(openFile.FileName, System.Text.Encoding.Default))
                    { 
                        string line = "";
                        while((line = sr.ReadLine()) != null)
                        {
                            //lbUserList.Items.Add(line.Trim());
                            AddUserList(line.Trim());
                        }
                    }
                
                }
            }
        }

       
        /// <summary>
        /// 당첨자는 숫자만 입력 받음
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbWinner_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 뽑기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //목록 길이
                int iListCount = lbUserList.Items.Count;
                if (iListCount < 1)
                {
                    MessageBox.Show("대상자가 없습니다", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Random rval = new Random();
                
                int iWinnerCnt = Convert.ToInt32(tbWinner.Text);
                //string[] aWinner = new string[iWinnerCnt];
                string sWinner = "";

                for (int i = 0; i < iWinnerCnt; i++)
                {
                    int iWinnerNum = rval.Next(0, iListCount);
                    //aWinner.SetValue(lbUserList.Items[iWinnerNum].ToString(), i);
                    //당첨자가 있으면 다음으로 넘어감
                    if (sWinName.IndexOf(lbUserList.Items[iWinnerNum].ToString()) > 0)
                    {
                        i = i - 1;
                    }
                    else
                    {
                        sWinner += lbUserList.Items[iWinnerNum].ToString() + "\r\n";
                        sWinName += lbUserList.Items[iWinnerNum].ToString()+";";
                    }
                    
                }
                Thread.Sleep(500);
                //당첨이 끝났으면 당첨자 목록 지우기
                sWinName = "";
                MessageBox.Show(sWinner, "당첨자");
            }
            catch (Exception)
            { 
            
            }
        }

        /// <summary>
        /// 화면캡처
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
           // CaptureScreen(
        }

        private void Mainform_Load(object sender, EventArgs e)
        {
            lbUserList.Items.Clear();
        }

        /// <summary>
        /// 엔터면 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbAdd_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter) && tbAdd.Text.Length > 0)
            {
                //lbUserList.Items.Add(tbAdd.Text.Trim());
                //tbAdd.Text = "";
                string sResult = AddUserList(tbAdd.Text.Trim());
                if(sResult != "add")
                {
                    MessageBox.Show(sResult);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// 대상자 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbUserList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (lbUserList.SelectedItem != null && lbUserList.SelectedItem.ToString().Length > 0)
                {

                    if (DialogResult.OK == MessageBox.Show("선택된 대상자를 삭제할까요?", "대상자 삭제", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                    {
                        lbUserList.Items.RemoveAt(lbUserList.SelectedIndex);
                        lbUserList.Refresh();
                    }
                }

            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// 사용자 리스트 추가
        /// </summary>
        /// <param name="userName"></param>
        private string AddUserList(string userName)
        {
            string sReturn = "";

            //이미 있는지 확인할 것
            if (lbUserList.Items.IndexOf(userName) > 0)
            {
                sReturn = "이미 존재합니다";
                return sReturn;
            }
            lbUserList.Items.Add(userName);
            tbAdd.Text = "";
            sReturn = "add";
            return sReturn;
        }

        /// <summary>
        /// 초기화 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            tbAdd.Text = "";
            lbUserList.Items.Clear();
            sWinName = "";
        }
    }
}
