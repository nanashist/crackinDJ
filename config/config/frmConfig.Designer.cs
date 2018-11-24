using System.Collections.Generic;

partial class frmConfig
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            this.cboAsioDriver = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtASIOoffset = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.trkFader = new System.Windows.Forms.TrackBar();
            this.lstMIDIEvent = new System.Windows.Forms.ListBox();
            this.btnLTurnL = new System.Windows.Forms.Button();
            this.btnLTouch = new System.Windows.Forms.Button();
            this.btnLTurnR = new System.Windows.Forms.Button();
            this.btnFadeL = new System.Windows.Forms.Button();
            this.btnFadeR = new System.Windows.Forms.Button();
            this.btnFadeC = new System.Windows.Forms.Button();
            this.btnRTurnR = new System.Windows.Forms.Button();
            this.btnRTouch = new System.Windows.Forms.Button();
            this.btnRTurnL = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cboMIDIINDEVICE = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.btnRecord = new System.Windows.Forms.Button();
            this.btnMIDIIn = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnListClear = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFader)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboAsioDriver
            // 
            this.cboAsioDriver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAsioDriver.FormattingEnabled = true;
            this.cboAsioDriver.Location = new System.Drawing.Point(98, 6);
            this.cboAsioDriver.Name = "cboAsioDriver";
            this.cboAsioDriver.Size = new System.Drawing.Size(194, 20);
            this.cboAsioDriver.TabIndex = 0;
            this.cboAsioDriver.SelectedIndexChanged += new System.EventHandler(this.cboAsioDriver_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "ASIOデバイス";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(298, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "ch";
            // 
            // txtASIOoffset
            // 
            this.txtASIOoffset.Location = new System.Drawing.Point(321, 7);
            this.txtASIOoffset.Name = "txtASIOoffset";
            this.txtASIOoffset.Size = new System.Drawing.Size(19, 19);
            this.txtASIOoffset.TabIndex = 3;
            this.txtASIOoffset.Text = "0";
            this.txtASIOoffset.TextChanged += new System.EventHandler(this.txtASIOoffset_TextChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 200);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(328, 28);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(200, 200);
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // trkFader
            // 
            this.trkFader.Location = new System.Drawing.Point(218, 183);
            this.trkFader.Maximum = 127;
            this.trkFader.Name = "trkFader";
            this.trkFader.Size = new System.Drawing.Size(104, 45);
            this.trkFader.TabIndex = 6;
            this.trkFader.TickFrequency = 64;
            // 
            // lstMIDIEvent
            // 
            this.lstMIDIEvent.FormattingEnabled = true;
            this.lstMIDIEvent.ItemHeight = 12;
            this.lstMIDIEvent.Location = new System.Drawing.Point(566, 89);
            this.lstMIDIEvent.Name = "lstMIDIEvent";
            this.lstMIDIEvent.Size = new System.Drawing.Size(154, 244);
            this.lstMIDIEvent.TabIndex = 7;
            // 
            // btnLTurnL
            // 
            this.btnLTurnL.BackColor = System.Drawing.Color.Gold;
            this.btnLTurnL.Location = new System.Drawing.Point(11, 234);
            this.btnLTurnL.Name = "btnLTurnL";
            this.btnLTurnL.Size = new System.Drawing.Size(62, 24);
            this.btnLTurnL.TabIndex = 8;
            this.btnLTurnL.Text = "←";
            this.btnLTurnL.UseVisualStyleBackColor = true;
            this.btnLTurnL.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnLTouch
            // 
            this.btnLTouch.Location = new System.Drawing.Point(80, 234);
            this.btnLTouch.Name = "btnLTouch";
            this.btnLTouch.Size = new System.Drawing.Size(62, 24);
            this.btnLTouch.TabIndex = 9;
            this.btnLTouch.Text = "Touch";
            this.btnLTouch.UseVisualStyleBackColor = true;
            this.btnLTouch.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnLTurnR
            // 
            this.btnLTurnR.Location = new System.Drawing.Point(150, 234);
            this.btnLTurnR.Name = "btnLTurnR";
            this.btnLTurnR.Size = new System.Drawing.Size(62, 24);
            this.btnLTurnR.TabIndex = 10;
            this.btnLTurnR.Text = "→";
            this.btnLTurnR.UseVisualStyleBackColor = true;
            this.btnLTurnR.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnFadeL
            // 
            this.btnFadeL.Location = new System.Drawing.Point(218, 234);
            this.btnFadeL.Margin = new System.Windows.Forms.Padding(1);
            this.btnFadeL.Name = "btnFadeL";
            this.btnFadeL.Size = new System.Drawing.Size(34, 24);
            this.btnFadeL.TabIndex = 11;
            this.btnFadeL.Text = "←";
            this.btnFadeL.UseVisualStyleBackColor = true;
            this.btnFadeL.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnFadeR
            // 
            this.btnFadeR.Location = new System.Drawing.Point(288, 234);
            this.btnFadeR.Margin = new System.Windows.Forms.Padding(1);
            this.btnFadeR.Name = "btnFadeR";
            this.btnFadeR.Size = new System.Drawing.Size(34, 24);
            this.btnFadeR.TabIndex = 12;
            this.btnFadeR.Text = "→";
            this.btnFadeR.UseVisualStyleBackColor = true;
            this.btnFadeR.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnFadeC
            // 
            this.btnFadeC.Location = new System.Drawing.Point(254, 234);
            this.btnFadeC.Margin = new System.Windows.Forms.Padding(1);
            this.btnFadeC.Name = "btnFadeC";
            this.btnFadeC.Size = new System.Drawing.Size(32, 24);
            this.btnFadeC.TabIndex = 13;
            this.btnFadeC.Text = "｜";
            this.btnFadeC.UseVisualStyleBackColor = true;
            this.btnFadeC.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnRTurnR
            // 
            this.btnRTurnR.Location = new System.Drawing.Point(467, 234);
            this.btnRTurnR.Name = "btnRTurnR";
            this.btnRTurnR.Size = new System.Drawing.Size(62, 24);
            this.btnRTurnR.TabIndex = 16;
            this.btnRTurnR.Text = "→";
            this.btnRTurnR.UseVisualStyleBackColor = true;
            this.btnRTurnR.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnRTouch
            // 
            this.btnRTouch.Location = new System.Drawing.Point(397, 234);
            this.btnRTouch.Name = "btnRTouch";
            this.btnRTouch.Size = new System.Drawing.Size(62, 24);
            this.btnRTouch.TabIndex = 15;
            this.btnRTouch.Text = "Touch";
            this.btnRTouch.UseVisualStyleBackColor = true;
            this.btnRTouch.Click += new System.EventHandler(this.btn_Click);
            // 
            // btnRTurnL
            // 
            this.btnRTurnL.Location = new System.Drawing.Point(328, 234);
            this.btnRTurnL.Name = "btnRTurnL";
            this.btnRTurnL.Size = new System.Drawing.Size(62, 24);
            this.btnRTurnL.TabIndex = 14;
            this.btnRTurnL.Text = "←";
            this.btnRTurnL.UseVisualStyleBackColor = true;
            this.btnRTurnL.Click += new System.EventHandler(this.btn_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(660, 339);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(60, 23);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "終了";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(594, 339);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 23);
            this.btnSave.TabIndex = 18;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cboMIDIINDEVICE
            // 
            this.cboMIDIINDEVICE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMIDIINDEVICE.FormattingEnabled = true;
            this.cboMIDIINDEVICE.Location = new System.Drawing.Point(98, 32);
            this.cboMIDIINDEVICE.Name = "cboMIDIINDEVICE";
            this.cboMIDIINDEVICE.Size = new System.Drawing.Size(194, 20);
            this.cboMIDIINDEVICE.TabIndex = 19;
            this.cboMIDIINDEVICE.SelectedIndexChanged += new System.EventHandler(this.cboMIDIINDEVICE_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "MIDI INデバイス";
            // 
            // timer2
            // 
            this.timer2.Interval = 10;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // btnRecord
            // 
            this.btnRecord.Location = new System.Drawing.Point(480, 35);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(75, 23);
            this.btnRecord.TabIndex = 21;
            this.btnRecord.Text = "レコード回転";
            this.btnRecord.UseVisualStyleBackColor = true;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // btnMIDIIn
            // 
            this.btnMIDIIn.Location = new System.Drawing.Point(566, 7);
            this.btnMIDIIn.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnMIDIIn.Name = "btnMIDIIn";
            this.btnMIDIIn.Size = new System.Drawing.Size(154, 20);
            this.btnMIDIIn.TabIndex = 22;
            this.btnMIDIIn.Text = "MIDI入力";
            this.btnMIDIIn.UseVisualStyleBackColor = true;
            this.btnMIDIIn.Click += new System.EventHandler(this.btnMIDIIn_Click);
            // 
            // btnListClear
            // 
            this.btnListClear.Location = new System.Drawing.Point(670, 66);
            this.btnListClear.Name = "btnListClear";
            this.btnListClear.Size = new System.Drawing.Size(50, 20);
            this.btnListClear.TabIndex = 23;
            this.btnListClear.Text = "クリア";
            this.btnListClear.UseVisualStyleBackColor = true;
            this.btnListClear.Click += new System.EventHandler(this.btnListClear_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.pictureBox2);
            this.groupBox1.Controls.Add(this.trkFader);
            this.groupBox1.Controls.Add(this.btnLTurnL);
            this.groupBox1.Controls.Add(this.btnLTouch);
            this.groupBox1.Controls.Add(this.btnLTurnR);
            this.groupBox1.Controls.Add(this.btnFadeL);
            this.groupBox1.Controls.Add(this.btnFadeR);
            this.groupBox1.Controls.Add(this.btnRTurnR);
            this.groupBox1.Controls.Add(this.btnFadeC);
            this.groupBox1.Controls.Add(this.btnRTouch);
            this.groupBox1.Controls.Add(this.btnRTurnL);
            this.groupBox1.Location = new System.Drawing.Point(8, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(547, 275);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MIDI入力割り当て";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(564, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 12);
            this.label4.TabIndex = 25;
            this.label4.Text = "MIDI INイベント";
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 382);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnListClear);
            this.Controls.Add(this.btnMIDIIn);
            this.Controls.Add(this.btnRecord);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboMIDIINDEVICE);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lstMIDIEvent);
            this.Controls.Add(this.txtASIOoffset);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboAsioDriver);
            this.Name = "frmConfig";
            this.Text = "frmConfig";
            this.Load += new System.EventHandler(this.frmConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFader)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox cboAsioDriver;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtASIOoffset;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.PictureBox pictureBox2;
    private System.Windows.Forms.TrackBar trkFader;
    private System.Windows.Forms.ListBox lstMIDIEvent;
    private System.Windows.Forms.Button btnLTurnL;
    private System.Windows.Forms.Button btnLTouch;
    private System.Windows.Forms.Button btnLTurnR;
    private System.Windows.Forms.Button btnFadeL;
    private System.Windows.Forms.Button btnFadeR;
    private System.Windows.Forms.Button btnFadeC;
    private System.Windows.Forms.Button btnRTurnR;
    private System.Windows.Forms.Button btnRTouch;
    private System.Windows.Forms.Button btnRTurnL;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.ComboBox cboMIDIINDEVICE;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Timer timer2;
    private System.Windows.Forms.Button btnRecord;
    private System.Windows.Forms.Button btnMIDIIn;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.Button btnListClear;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label label4;
}
