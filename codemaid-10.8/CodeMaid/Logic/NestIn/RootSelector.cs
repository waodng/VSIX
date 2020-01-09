using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/* ==============================================================================
 * 创建日期：2019/8/29 22:42:40
 * 创 建 者：wgd
 * 功能描述：RootSelector  
 * ==============================================================================*/
namespace SteveCadwallader.CodeMaid.Logic.NestIn
{
    public class RootSelector : Form, IRootSelector
    {
        // Token: 0x0600000C RID: 12 RVA: 0x000022D6 File Offset: 0x000004D6
        public RootSelector()
        {
            this.InitializeComponent();
        }

        // Token: 0x0600000D RID: 13 RVA: 0x000022F0 File Offset: 0x000004F0
        public FileItem Select(IEnumerable<FileItem> candidates)
        {
            this.comboFileItem.DataSource = candidates.ToList<FileItem>();
            this.comboFileItem.DisplayMember = "Name";
            FileItem result;
            if (base.ShowDialog() == DialogResult.OK)
            {
                result = (FileItem)this.comboFileItem.SelectedItem;
            }
            else
            {
                result = null;
            }
            return result;
        }

        // Token: 0x0600000E RID: 14 RVA: 0x0000234A File Offset: 0x0000054A
        private void OkButtonClick(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        // Token: 0x0600000F RID: 15 RVA: 0x0000235C File Offset: 0x0000055C
        private void CancelButtonClick(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        // Token: 0x06000010 RID: 16 RVA: 0x00002370 File Offset: 0x00000570
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Token: 0x06000011 RID: 17 RVA: 0x000023A8 File Offset: 0x000005A8
        private void InitializeComponent()
        {
            this.comboFileItem = new ComboBox();
            this.label1 = new Label();
            this.OkButton = new Button();
            this.CancelButton = new Button();
            base.SuspendLayout();
            this.comboFileItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboFileItem.FormattingEnabled = true;
            this.comboFileItem.Location = new Point(94, 14);
            this.comboFileItem.Name = "comboFileItem";
            this.comboFileItem.Size = new Size(203, 21);
            this.comboFileItem.TabIndex = 0;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select a Root:";
            this.OkButton.DialogResult = DialogResult.OK;
            this.OkButton.Location = new Point(318, 12);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new Size(75, 23);
            this.OkButton.TabIndex = 2;
            this.OkButton.Text = "&OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += this.OkButtonClick;
            this.CancelButton.DialogResult = DialogResult.Cancel;
            this.CancelButton.Location = new Point(318, 41);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new Size(75, 23);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "&Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += this.CancelButtonClick;
            base.AcceptButton = this.OkButton;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(405, 69);
            base.Controls.Add(this.CancelButton);
            base.Controls.Add(this.OkButton);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.comboFileItem);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "RootSelector";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Nest In!";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        // Token: 0x04000008 RID: 8
        private IContainer components = null;

        // Token: 0x04000009 RID: 9
        private ComboBox comboFileItem;

        // Token: 0x0400000A RID: 10
        private Label label1;

        // Token: 0x0400000B RID: 11
        private Button OkButton;

        // Token: 0x0400000C RID: 12
        private new Button CancelButton;
    }
}
