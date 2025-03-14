
namespace морской_бой
{
    partial class ConnectForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_Enter = new System.Windows.Forms.Button();
            this.IP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rb_Client = new System.Windows.Forms.RadioButton();
            this.rb_Server = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btn_Enter
            // 
            this.btn_Enter.Location = new System.Drawing.Point(44, 189);
            this.btn_Enter.Name = "btn_Enter";
            this.btn_Enter.Size = new System.Drawing.Size(228, 30);
            this.btn_Enter.TabIndex = 0;
            this.btn_Enter.Text = "Передать";
            this.btn_Enter.UseVisualStyleBackColor = true;
            this.btn_Enter.Click += new System.EventHandler(this.btn_Enter_Click1);
            // 
            // IP
            // 
            this.IP.Location = new System.Drawing.Point(64, 31);
            this.IP.Name = "IP";
            this.IP.Size = new System.Drawing.Size(228, 20);
            this.IP.TabIndex = 1;
            this.IP.Text = "192.168.31.86";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "IP";
            // 
            // rb_Client
            // 
            this.rb_Client.AutoSize = true;
            this.rb_Client.Location = new System.Drawing.Point(54, 97);
            this.rb_Client.Name = "rb_Client";
            this.rb_Client.Size = new System.Drawing.Size(51, 17);
            this.rb_Client.TabIndex = 5;
            this.rb_Client.TabStop = true;
            this.rb_Client.Text = "Client";
            this.rb_Client.UseVisualStyleBackColor = true;
            this.rb_Client.CheckedChanged += new System.EventHandler(this.rb_Client_CheckedChanged);
            // 
            // rb_Server
            // 
            this.rb_Server.AutoSize = true;
            this.rb_Server.Location = new System.Drawing.Point(54, 120);
            this.rb_Server.Name = "rb_Server";
            this.rb_Server.Size = new System.Drawing.Size(56, 17);
            this.rb_Server.TabIndex = 5;
            this.rb_Server.TabStop = true;
            this.rb_Server.Text = "Server";
            this.rb_Server.UseVisualStyleBackColor = true;
            this.rb_Server.CheckedChanged += new System.EventHandler(this.rb_Server_CheckedChanged);
            // 
            // ConnectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 247);
            this.Controls.Add(this.rb_Server);
            this.Controls.Add(this.rb_Client);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IP);
            this.Controls.Add(this.btn_Enter);
            this.Name = "ConnectForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Enter;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.RadioButton rb_Client;
        public System.Windows.Forms.RadioButton rb_Server;
        public System.Windows.Forms.TextBox IP;
    }
}

