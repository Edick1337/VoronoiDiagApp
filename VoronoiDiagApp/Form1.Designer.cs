namespace VoronoiDiagApp
{
    partial class Form1
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
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.pcb_Diagram = new System.Windows.Forms.PictureBox();
            this.btn_Start = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btn_SchoolsData = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_Diagram)).BeginInit();
            this.SuspendLayout();
            // 
            // pcb_Diagram
            // 
            this.pcb_Diagram.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pcb_Diagram.BackColor = System.Drawing.Color.Black;
            this.pcb_Diagram.InitialImage = null;
            this.pcb_Diagram.Location = new System.Drawing.Point(12, 60);
            this.pcb_Diagram.Name = "pcb_Diagram";
            this.pcb_Diagram.Size = new System.Drawing.Size(916, 526);
            this.pcb_Diagram.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pcb_Diagram.TabIndex = 0;
            this.pcb_Diagram.TabStop = false;
            this.pcb_Diagram.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pcb_Diagram_MouseClick);
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(12, 12);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(75, 23);
            this.btn_Start.TabIndex = 1;
            this.btn_Start.Text = "Calculate";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 100;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btn_SchoolsData_Click
            // 
            this.btn_SchoolsData.Location = new System.Drawing.Point(135, 12);
            this.btn_SchoolsData.Name = "btn_SchoolsData";
            this.btn_SchoolsData.Size = new System.Drawing.Size(115, 23);
            this.btn_SchoolsData.TabIndex = 2;
            this.btn_SchoolsData.Text = "Schools data";
            this.btn_SchoolsData.UseVisualStyleBackColor = true;
            this.btn_SchoolsData.Click += new System.EventHandler(this.btn_SchoolsData_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 598);
            this.Controls.Add(this.btn_SchoolsData);
            this.Controls.Add(this.btn_Start);
            this.Controls.Add(this.pcb_Diagram);
            this.Name = "Form1";
            this.Text = "Voronoi Diagram for Mariupol Schools";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pcb_Diagram)).EndInit();
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.PictureBox pcb_Diagram;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btn_SchoolsData;
    }
}