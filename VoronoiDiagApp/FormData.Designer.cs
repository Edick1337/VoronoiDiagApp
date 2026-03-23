namespace VoronoiDiagApp
{
    partial class FormData
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SchoolNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SchoolLatitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SchoolLongtitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Occupancy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.SchoolNumber, this.SchoolLatitude, this.SchoolLongtitude, this.Occupancy });
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(380, 516);
            this.dataGridView1.TabIndex = 0;
            // 
            // SchoolNumber
            // 
            this.SchoolNumber.HeaderText = "School #";
            this.SchoolNumber.Name = "SchoolNumber";
            this.SchoolNumber.Width = 80;
            // 
            // SchoolLatitude
            // 
            this.SchoolLatitude.HeaderText = "Latitude";
            this.SchoolLatitude.Name = "SchoolLatitude";
            this.SchoolLatitude.Width = 80;
            // 
            // SchoolLongtitude
            // 
            this.SchoolLongtitude.HeaderText = "Longitude";
            this.SchoolLongtitude.Name = "SchoolLongtitude";
            this.SchoolLongtitude.Width = 80;
            // 
            // Occupancy
            // 
            this.Occupancy.HeaderText = "Occupancy";
            this.Occupancy.Name = "Occupancy";
            // 
            // FormData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 516);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormData";
            this.Text = "Schools data";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridViewTextBoxColumn Occupancy;

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SchoolNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn SchoolLatitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn SchoolLongtitude;
    }
}