namespace Client
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
        private void InitializeComponent()
        {
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.registerButton = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.panelStart = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.buttonRemoveFriend = new System.Windows.Forms.Button();
            this.panelNotifications = new System.Windows.Forms.Panel();
            this.panelChat = new System.Windows.Forms.Panel();
            this.labelChatWith = new System.Windows.Forms.Label();
            this.buttonSendMessage = new System.Windows.Forms.Button();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.listBoxMessages = new System.Windows.Forms.ListBox();
            this.buttonRejectInvite = new System.Windows.Forms.Button();
            this.buttonAcceptInvite = new System.Windows.Forms.Button();
            this.listBoxInvites = new System.Windows.Forms.ListBox();
            this.listBoxFriends = new System.Windows.Forms.ListBox();
            this.buttonSendInvite = new System.Windows.Forms.Button();
            this.textBoxInvite = new System.Windows.Forms.TextBox();
            this.panelError = new System.Windows.Forms.Panel();
            this.labelError = new System.Windows.Forms.Label();
            this.panelStart.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelChat.SuspendLayout();
            this.panelError.SuspendLayout();
            this.SuspendLayout();
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.Location = new System.Drawing.Point(60, 42);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(168, 20);
            this.UsernameTextBox.TabIndex = 0;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(60, 81);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PasswordChar = '*';
            this.PasswordTextBox.Size = new System.Drawing.Size(168, 20);
            this.PasswordTextBox.TabIndex = 1;
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(60, 116);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 2;
            this.loginButton.Text = "Zaloguj się";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point(144, 116);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(84, 24);
            this.registerButton.TabIndex = 3;
            this.registerButton.Text = "Zarejestruj się";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            this.label1.Visible = false;
            // 
            // panelStart
            // 
            this.panelStart.Controls.Add(this.label3);
            this.panelStart.Controls.Add(this.label2);
            this.panelStart.Controls.Add(this.UsernameTextBox);
            this.panelStart.Controls.Add(this.label1);
            this.panelStart.Controls.Add(this.PasswordTextBox);
            this.panelStart.Controls.Add(this.registerButton);
            this.panelStart.Controls.Add(this.loginButton);
            this.panelStart.Location = new System.Drawing.Point(271, 87);
            this.panelStart.Name = "panelStart";
            this.panelStart.Size = new System.Drawing.Size(258, 234);
            this.panelStart.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "hasło";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "login";
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.buttonRemoveFriend);
            this.panelMain.Controls.Add(this.panelNotifications);
            this.panelMain.Controls.Add(this.panelChat);
            this.panelMain.Controls.Add(this.buttonRejectInvite);
            this.panelMain.Controls.Add(this.buttonAcceptInvite);
            this.panelMain.Controls.Add(this.listBoxInvites);
            this.panelMain.Controls.Add(this.listBoxFriends);
            this.panelMain.Controls.Add(this.buttonSendInvite);
            this.panelMain.Controls.Add(this.textBoxInvite);
            this.panelMain.Location = new System.Drawing.Point(12, 13);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(783, 425);
            this.panelMain.TabIndex = 6;
            this.panelMain.Visible = false;
            // 
            // buttonRemoveFriend
            // 
            this.buttonRemoveFriend.Location = new System.Drawing.Point(4, 340);
            this.buttonRemoveFriend.Name = "buttonRemoveFriend";
            this.buttonRemoveFriend.Size = new System.Drawing.Size(108, 23);
            this.buttonRemoveFriend.TabIndex = 14;
            this.buttonRemoveFriend.Text = "Usuń";
            this.buttonRemoveFriend.UseVisualStyleBackColor = true;
            this.buttonRemoveFriend.Click += new System.EventHandler(this.buttonRemoveFriend_Click);
            // 
            // panelNotifications
            // 
            this.panelNotifications.Location = new System.Drawing.Point(118, 380);
            this.panelNotifications.Name = "panelNotifications";
            this.panelNotifications.Size = new System.Drawing.Size(529, 42);
            this.panelNotifications.TabIndex = 13;
            // 
            // panelChat
            // 
            this.panelChat.Controls.Add(this.labelChatWith);
            this.panelChat.Controls.Add(this.buttonSendMessage);
            this.panelChat.Controls.Add(this.textBoxMessage);
            this.panelChat.Controls.Add(this.listBoxMessages);
            this.panelChat.Location = new System.Drawing.Point(118, 3);
            this.panelChat.Name = "panelChat";
            this.panelChat.Size = new System.Drawing.Size(529, 370);
            this.panelChat.TabIndex = 12;
            this.panelChat.Visible = false;
            // 
            // labelChatWith
            // 
            this.labelChatWith.AutoSize = true;
            this.labelChatWith.Location = new System.Drawing.Point(12, 4);
            this.labelChatWith.Name = "labelChatWith";
            this.labelChatWith.Size = new System.Drawing.Size(35, 13);
            this.labelChatWith.TabIndex = 14;
            this.labelChatWith.Text = "label4";
            // 
            // buttonSendMessage
            // 
            this.buttonSendMessage.Location = new System.Drawing.Point(438, 337);
            this.buttonSendMessage.Name = "buttonSendMessage";
            this.buttonSendMessage.Size = new System.Drawing.Size(75, 23);
            this.buttonSendMessage.TabIndex = 13;
            this.buttonSendMessage.Text = "Wyślij";
            this.buttonSendMessage.UseVisualStyleBackColor = true;
            this.buttonSendMessage.Click += new System.EventHandler(this.buttonSendMessage_Click);
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(12, 339);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(420, 20);
            this.textBoxMessage.TabIndex = 12;
            this.textBoxMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxMessage_KeyPress);
            // 
            // listBoxMessages
            // 
            this.listBoxMessages.FormattingEnabled = true;
            this.listBoxMessages.Location = new System.Drawing.Point(12, 29);
            this.listBoxMessages.Name = "listBoxMessages";
            this.listBoxMessages.Size = new System.Drawing.Size(499, 303);
            this.listBoxMessages.TabIndex = 11;
            // 
            // buttonRejectInvite
            // 
            this.buttonRejectInvite.Location = new System.Drawing.Point(653, 202);
            this.buttonRejectInvite.Name = "buttonRejectInvite";
            this.buttonRejectInvite.Size = new System.Drawing.Size(119, 23);
            this.buttonRejectInvite.TabIndex = 10;
            this.buttonRejectInvite.Text = "Odrzuć zaproszenie";
            this.buttonRejectInvite.UseVisualStyleBackColor = true;
            this.buttonRejectInvite.Click += new System.EventHandler(this.buttonRejectInvite_Click);
            // 
            // buttonAcceptInvite
            // 
            this.buttonAcceptInvite.Location = new System.Drawing.Point(653, 172);
            this.buttonAcceptInvite.Name = "buttonAcceptInvite";
            this.buttonAcceptInvite.Size = new System.Drawing.Size(119, 23);
            this.buttonAcceptInvite.TabIndex = 9;
            this.buttonAcceptInvite.Text = "Akceptuj zaproszenie";
            this.buttonAcceptInvite.UseVisualStyleBackColor = true;
            this.buttonAcceptInvite.Click += new System.EventHandler(this.buttonAcceptInvite_Click);
            // 
            // listBoxInvites
            // 
            this.listBoxInvites.FormattingEnabled = true;
            this.listBoxInvites.Location = new System.Drawing.Point(652, 32);
            this.listBoxInvites.Name = "listBoxInvites";
            this.listBoxInvites.Size = new System.Drawing.Size(120, 134);
            this.listBoxInvites.TabIndex = 8;
            // 
            // listBoxFriends
            // 
            this.listBoxFriends.FormattingEnabled = true;
            this.listBoxFriends.Location = new System.Drawing.Point(4, 32);
            this.listBoxFriends.Name = "listBoxFriends";
            this.listBoxFriends.Size = new System.Drawing.Size(107, 303);
            this.listBoxFriends.TabIndex = 7;
            this.listBoxFriends.SelectedIndexChanged += new System.EventHandler(this.listBoxFriends_SelectedIndexChanged);
            // 
            // buttonSendInvite
            // 
            this.buttonSendInvite.Location = new System.Drawing.Point(652, 277);
            this.buttonSendInvite.Name = "buttonSendInvite";
            this.buttonSendInvite.Size = new System.Drawing.Size(120, 23);
            this.buttonSendInvite.TabIndex = 2;
            this.buttonSendInvite.Text = "Wyślij zaproszenie";
            this.buttonSendInvite.UseVisualStyleBackColor = true;
            this.buttonSendInvite.Click += new System.EventHandler(this.buttonSendInvite_Click);
            // 
            // textBoxInvite
            // 
            this.textBoxInvite.Location = new System.Drawing.Point(652, 251);
            this.textBoxInvite.Name = "textBoxInvite";
            this.textBoxInvite.Size = new System.Drawing.Size(120, 20);
            this.textBoxInvite.TabIndex = 1;
            // 
            // panelError
            // 
            this.panelError.Controls.Add(this.labelError);
            this.panelError.Location = new System.Drawing.Point(136, 71);
            this.panelError.Name = "panelError";
            this.panelError.Size = new System.Drawing.Size(571, 274);
            this.panelError.TabIndex = 13;
            this.panelError.Visible = false;
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelError.Location = new System.Drawing.Point(3, 114);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(563, 25);
            this.labelError.TabIndex = 0;
            this.labelError.Text = "Nie można nawiązać połączenia, zrestartuj aplikacje";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelError);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelStart);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panelStart.ResumeLayout(false);
            this.panelStart.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panelChat.ResumeLayout(false);
            this.panelChat.PerformLayout();
            this.panelError.ResumeLayout(false);
            this.panelError.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox UsernameTextBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Button registerButton;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Button buttonSendInvite;
        private System.Windows.Forms.TextBox textBoxInvite;
        private System.Windows.Forms.ListBox listBoxFriends;
        private System.Windows.Forms.ListBox listBoxInvites;
        private System.Windows.Forms.Button buttonAcceptInvite;
        private System.Windows.Forms.Button buttonRejectInvite;
        private System.Windows.Forms.ListBox listBoxMessages;
        private System.Windows.Forms.Panel panelChat;
        private System.Windows.Forms.Button buttonSendMessage;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Label labelChatWith;
        private System.Windows.Forms.Panel panelError;
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.Panel panelNotifications;
        private System.Windows.Forms.Button buttonRemoveFriend;
    }
}

