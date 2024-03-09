using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OutlawTool
{
    public partial class Form1 : Form
    {
        private string DefaultRedmPath = null;
        private string DefaultFivemPath = null;
        private bool isFirstRun = true;
        private bool hasFilePermission = false;
        private readonly string currentUser = Environment.UserName;


        public Form1()
        {
            InitializeComponent();
            DefaultRedmPath = Path.Combine("C:\\Users", currentUser, "AppData", "Local", "RedM");
            DefaultFivemPath = Path.Combine("C:\\Users", currentUser, "AppData", "Local", "FiveM");

            isFirstRun = Properties.Settings.Default.IsFirstRun;
            if (isFirstRun)
            {
                RequestFilePermissions();
                Properties.Settings.Default.IsFirstRun = false;
                Properties.Settings.Default.Save();
            }
        }

        private void RequestFilePermissions()
        {
            try
            {
                string testFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.txt");
                File.WriteAllText(testFilePath, "Test");
                File.Delete(testFilePath);
                hasFilePermission = true;
                DialogResult result = MessageBox.Show("L'application a été lancée de sorte à modifier les fichiers RedM & FiveM. Voulez-vous accorder les permissions suffisante pour pouvoir continuer ?", "Permissions de modification obligatoire", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            
                if(result == DialogResult.No) {
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Vous n'avez pas les autorisations nécessaires pour modifier les fichiers. Veuillez accorder les autorisations nécessaires pour utiliser toutes les fonctionnalités de l'application. {ex}", "Autorisations de fichier insuffisantes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                hasFilePermission = false;
                Environment.Exit(0);
            }
        }

        private void RedmToAzerty_Click(object sender, EventArgs e)
        {
            try
            {
                string DefaultCommonMetaPath = Path.Combine(DefaultRedmPath, "RedM.app", "citizen", "common", "data", "control", "default.meta");
                string DefaultPlatformMetaPath = Path.Combine(DefaultRedmPath, "RedM.app", "citizen", "platform", "data", "control", "default.meta");
                string SettingsCommonMetaPath = Path.Combine(DefaultRedmPath, "RedM.app", "citizen", "common", "data", "control", "settings.meta");
                string SettingsPlatformMetaPath = Path.Combine(DefaultRedmPath, "RedM.app", "citizen", "platform", "data", "control", "settings.meta");

                using (WebClient client = new WebClient())
                {
                    client.DownloadFile("https://cdn.codoly.fr/p/default.meta", DefaultCommonMetaPath);
                    client.DownloadFile("https://cdn.codoly.fr/p/default.meta", DefaultPlatformMetaPath);
                    client.DownloadFile("https://cdn.codoly.fr/p/settings.meta", SettingsCommonMetaPath);
                    client.DownloadFile("https://cdn.codoly.fr/p/settings.meta", SettingsPlatformMetaPath);
                }
                
                MessageBox.Show("Les fichiers 'default.meta' et 'settings.meta' ont été remplacés avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors du téléchargement du fichier : {ex}.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearCacheRedm_Click(object sender, EventArgs e)
        {
            try
            {
                progressBar.Visible = true;
                string DefaultRedmCachePath = Path.Combine(DefaultRedmPath, "RedM.app", "data");

                if (Directory.Exists(DefaultRedmCachePath))
                {
                    bool hasGameStorage = false;
                    foreach (var folder in Directory.GetDirectories(DefaultRedmCachePath))
                    {
                        if (Path.GetFileName(folder) == "game-storage")
                        {
                            hasGameStorage = true;
                            continue;
                        }
                    }

                    if (hasGameStorage)
                    {
                        progressBar.Value = 0;
                        progressBar.Maximum = Directory.GetDirectories(DefaultRedmCachePath).Length;
                        int folderCount = 0;
                        foreach (var folder in Directory.GetDirectories(DefaultRedmCachePath))
                        {
                            if (Path.GetFileName(folder) != "game-storage")
                            {
                                Directory.Delete(folder, true);
                            }
                            progressBar.Value = ++folderCount;
                        }
                        MessageBox.Show("Cache RedM nettoyé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Le dossier de cache RedM ne contient pas de dossier \"game-storage\".", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Le dossier de cache RedM n'a pas été trouvé.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors du vidage du cache RedM, assurez-vous de fermer RedM avant de cliquer. Détails : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar.Visible = false;
            }
        }

        private void ClearCacheFivem_Click(object sender, EventArgs e)
        {
            try
            {
                progressBar.Visible = true;
                string DefaultFivemCachePath = Path.Combine(DefaultFivemPath, "FiveM.app", "data");

                if (Directory.Exists(DefaultFivemCachePath))
                {
                    bool hasGameStorage = false;
                    foreach (var folder in Directory.GetDirectories(DefaultFivemCachePath))
                    {
                        if (Path.GetFileName(folder) == "game-storage")
                        {
                            hasGameStorage = true;
                            continue;
                        }
                    }

                    if (hasGameStorage)
                    {
                        progressBar.Value = 0;
                        progressBar.Maximum = Directory.GetDirectories(DefaultFivemCachePath).Length;
                        int folderCount = 0;
                        foreach (var folder in Directory.GetDirectories(DefaultFivemCachePath))
                        {
                            if (Path.GetFileName(folder) != "game-storage")
                            {
                                Directory.Delete(folder, true);
                            }
                            progressBar.Value = ++folderCount;
                        }
                        MessageBox.Show("Cache FiveM nettoyé avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Le dossier de cache FiveM ne contient pas de dossier \"game-storage\".", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Le dossier de cache FiveM n'a pas été trouvé.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors du vidage du cache FiveM : {ex.Message}.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar.Visible = false;
            }
        }

        private void LaunchFivem_Click(object sender, EventArgs e)
        {
            try
            {
                string FiveMExecutablePath = Path.Combine(DefaultFivemPath, "FiveM.exe");
                if (File.Exists(FiveMExecutablePath))
                {
                    Process.Start(FiveMExecutablePath);
                }
                else
                {
                    MessageBox.Show("Le fichier exécutable FiveM n'a pas été trouvé.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors du démarrage de l'application FiveM : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LaunchRedm_Click(object sender, EventArgs e)
        {
            try
            {
                string RedMExecutablePath = Path.Combine(DefaultRedmPath, "RedM.exe");
                if (File.Exists(RedMExecutablePath))
                {
                    Process.Start(RedMExecutablePath);
                }
                else
                {
                    MessageBox.Show("Le fichier exécutable RedM n'a pas été trouvé.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors du démarrage de l'application RedM : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ouvrirLeDossierRedMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(DefaultRedmPath)) Process.Start("explorer.exe", DefaultRedmPath);
                else MessageBox.Show($"Le dossier RedM n'existe pas ou n'a pas été trouvé au chemin suivant : {DefaultRedmPath}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors de l'ouverture du fichier de RedM : {ex}\n\nChemin ciblé: {DefaultRedmPath}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ouvrirLeDossierFiveMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(DefaultFivemPath)) Process.Start("explorer.exe", DefaultFivemPath);
                else MessageBox.Show($"Le dossier FiveM n'existe pas ou n'a pas été trouvé au chemin suivant : {DefaultFivemPath}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors de l'ouverture du fichier de FiveM : {ex}\n\nChemin ciblé: {DefaultFivemPath}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void configurerLeCheminDeRedMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog folderBrowser = new OpenFileDialog())
            {
                folderBrowser.Title = "Sélectionner le chemin de RedM";
                folderBrowser.CheckFileExists = false;
                folderBrowser.FileName = "Sélectionner un dossier";
                folderBrowser.ValidateNames = false;
                folderBrowser.CheckPathExists = true;
                folderBrowser.FileName = "Dossier";

                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    DefaultRedmPath = Path.GetDirectoryName(folderBrowser.FileName);
                    Properties.Settings.Default.DefaultRedmPath = DefaultRedmPath;
                    Properties.Settings.Default.Save();
                    MessageBox.Show($"Le chemin par défaut de RedM a été défini sur : {DefaultRedmPath}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void configurerLeCheminDeFiveMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog folderBrowser = new OpenFileDialog())
            {
                folderBrowser.Title = "Sélectionner le chemin de FiveM";
                folderBrowser.CheckFileExists = false;
                folderBrowser.FileName = "Sélectionner un dossier";
                folderBrowser.ValidateNames = false;
                folderBrowser.CheckPathExists = true;
                folderBrowser.FileName = "Dossier";

                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    DefaultFivemPath = Path.GetDirectoryName(folderBrowser.FileName);
                    Properties.Settings.Default.DefaultFivemPath = DefaultFivemPath;
                    Properties.Settings.Default.Save(); 
                    MessageBox.Show($"Le chemin par défaut de FiveM a été défini sur : {DefaultFivemPath}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void aProposToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Application créée par Doly (https://codoly.fr).\n\nVersion: 1.1.0\n\nDate de mise à jour: 06/03/2024", "A propos de CFX Tool - Outlaw Version", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void configurerLeCheminDeRedMToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog folderBrowser = new OpenFileDialog())
            {
                folderBrowser.Title = "Sélectionner le chemin de RedM";
                folderBrowser.CheckFileExists = false;
                folderBrowser.FileName = "Sélectionner un dossier";
                folderBrowser.ValidateNames = false;
                folderBrowser.CheckPathExists = true;
                folderBrowser.FileName = "Dossier";

                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    DefaultRedmPath = Path.GetDirectoryName(folderBrowser.FileName);
                    Properties.Settings.Default.DefaultRedmPath = DefaultRedmPath;
                    Properties.Settings.Default.Save();
                    MessageBox.Show($"Le chemin par défaut de RedM a été défini sur : {DefaultRedmPath}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void voirLeCheminActuelDeRedMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Le chemin par défault de RedM actuellement est : \n{DefaultRedmPath}");
        }

        private void configurerLeCheminDeFiveMToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog folderBrowser = new OpenFileDialog())
            {
                folderBrowser.Title = "Sélectionner le chemin de FiveM";
                folderBrowser.CheckFileExists = false;
                folderBrowser.FileName = "Sélectionner un dossier";
                folderBrowser.ValidateNames = false;
                folderBrowser.CheckPathExists = true;
                folderBrowser.FileName = "Dossier";

                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    DefaultFivemPath = Path.GetDirectoryName(folderBrowser.FileName);
                    Properties.Settings.Default.DefaultFivemPath = DefaultFivemPath;
                    Properties.Settings.Default.Save();
                    MessageBox.Show($"Le chemin par défaut de FiveM a été défini sur : {DefaultFivemPath}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        
        private void voirLeCheminActuelDeFiveMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Le chemin par défault de RedM actuellement est : \n{DefaultFivemPath}");
        }
        
        private void remettreLeCheminDeRedMParDéfautToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DefaultRedmPath = Path.Combine("C:\\Users", currentUser, "AppData", "Local", "RedM");
            MessageBox.Show($"Le chemin par défaut de RedM a été défini sur : {DefaultRedmPath}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void réinitialiserLeCheminDeFiveMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DefaultFivemPath = Path.Combine("C:\\Users", currentUser, "AppData", "Local", "FiveM");
            MessageBox.Show($"Le chemin par défaut de FiveM a été défini sur : {DefaultFivemPath}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void codeSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://github.com/Dolyyyy/cfxtool");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConnectOutlaw_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://game.outlawrdr.fr");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenWiki_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://wiki.outlawrdr.fr");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void OpenSite_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://outlawrdr.fr");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenStreamers_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://outlawrdr.fr/tv");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
