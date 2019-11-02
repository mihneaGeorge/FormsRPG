using Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FormsRPG {
    public partial class FormsRPG : Form {
        private Player _player;
        private const string PLAYER_DATA_FILE_NAME = "PlayerData.xml";
        public FormsRPG() {
            InitializeComponent();

            //_player = PlayerDataMapper.CreateFromDatabase();

            if(_player == null) {
                if (File.Exists(PLAYER_DATA_FILE_NAME)) {
                    _player = Player.CreatePlayerFromXmlString(File.ReadAllText(PLAYER_DATA_FILE_NAME));
                } else {
                    _player = Player.CreateDefaultPlayer();
                }
            }

            lblHitpoints.DataBindings.Add("Text", _player, "CurrentHitPoints");
            lblGold.DataBindings.Add("Text", _player, "Gold");
            lblExperience.DataBindings.Add("Text", _player, "ExperiencePoints");
            lblLevel.DataBindings.Add("Text", _player, "Level");
            if (_player.CurrentArmor != null) {
                lblDefenseVal.DataBindings.Add("Text", _player.CurrentArmor, "Defense");
            } else {
                lblDefenseVal.Text = "0";
            }

            dgvInventory.RowHeadersVisible = false;
            dgvInventory.AutoGenerateColumns = false;

            dgvInventory.DataSource = _player.Inventory;

            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "Description"
            });

            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Quantity",
                DataPropertyName = "Quantity"
            });

            dgvQuests.RowHeadersVisible = false;
            dgvQuests.AutoGenerateColumns = false;

            dgvQuests.DataSource = _player.Quests;

            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "Name"
            });

            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Done?",
                DataPropertyName = "IsCompleted"
            });

            cboWeapons.DataSource = _player.Weapons;
            cboWeapons.DisplayMember = "Name";
            cboWeapons.ValueMember = "Id";

            if (_player.CurrentWeapon != null) {
                cboWeapons.SelectedItem = _player.CurrentWeapon;
            }

            cboWeapons.SelectedIndexChanged += cboWeapons_SelectedIndexChanged;

            cboArmors.DataSource = _player.Armors;
            cboArmors.DisplayMember = "Name";
            cboArmors.ValueMember = "Id";

            if (_player.CurrentArmor != null) {
                cboArmors.SelectedItem = _player.CurrentArmor;
            }

            cboArmors.SelectedIndexChanged += cboArmors_SelectedIndexChanged;

            cboPotions.DataSource = _player.Potions;
            cboPotions.DisplayMember = "Name";
            cboPotions.ValueMember = "Id";

            _player.PropertyChanged += PlayerOnPropertyChanged;
            _player.OnMessage += DisplayMessage;

            rtbLocation.Text += _player.CurrentLocation.Name;
            rtbLocation.Text += Environment.NewLine;
            rtbLocation.Text += _player.CurrentLocation.Description;

            btnNorth.Visible = (_player.CurrentLocation.LocationToNorth != null);
            btnEast.Visible = (_player.CurrentLocation.LocationToEast != null);
            btnSouth.Visible = (_player.CurrentLocation.LocationToSouth != null);
            btnWest.Visible = (_player.CurrentLocation.LocationToWest != null);

            if (_player.CurrentLocation.MonsterLivingHere == null) {
                btnUseWeapon.Visible = false;
                btnUsePotion.Visible = false;
                cboPotions.Visible = false;
                cboWeapons.Visible = false;
            }

            //MoveTo(_player.CurrentLocation);
        }

        private void DisplayMessage(object sender, MessageEventArgs messageEventArgs) {
            rtbMessages.Text += messageEventArgs.Message + Environment.NewLine;

            if (messageEventArgs.AddExtraNewLine) {
                rtbMessages.Text += Environment.NewLine;
            }

            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }

        private void PlayerOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs) {
            if (propertyChangedEventArgs.PropertyName == "Weapons") {
                cboWeapons.DataSource = _player.Weapons;

                if (!_player.Weapons.Any()) {
                    cboWeapons.Visible = false;
                    btnUseWeapon.Visible = false;
                }
            }

            if (propertyChangedEventArgs.PropertyName == "Potions") {
                cboPotions.DataSource = _player.Potions;

                if (!_player.Potions.Any()) {
                    cboPotions.Visible = false;
                    btnUsePotion.Visible = false;
                }
            }

            if (propertyChangedEventArgs.PropertyName == "Armors") {
                cboArmors.DataSource = _player.Armors;
            }

            if (propertyChangedEventArgs.PropertyName == "CurrentLocation") {
                btnNorth.Visible = (_player.CurrentLocation.LocationToNorth != null);
                btnEast.Visible = (_player.CurrentLocation.LocationToEast != null);
                btnSouth.Visible = (_player.CurrentLocation.LocationToSouth != null);
                btnWest.Visible = (_player.CurrentLocation.LocationToWest != null);
                btnTrade.Visible = (_player.CurrentLocation.VendorWorkingHere != null);

                rtbLocation.Text = _player.CurrentLocation.Name + Environment.NewLine;
                rtbLocation.Text += _player.CurrentLocation.Description + Environment.NewLine;

                if (_player.CurrentLocation.MonsterLivingHere == null) {
                    cboWeapons.Visible = false;
                    cboPotions.Visible = false;
                    btnUseWeapon.Visible = false;
                    btnUsePotion.Visible = false;
                } else {
                    cboWeapons.Visible = _player.Weapons.Any();
                    cboPotions.Visible = _player.Potions.Any();
                    btnUseWeapon.Visible = _player.Weapons.Any();
                    btnUsePotion.Visible = _player.Potions.Any();
                }
            }
        }

        private void BtnNorth_Click(object sender, EventArgs e) {
            _player.MoveNorth();
        }

        private void BtnEast_Click(object sender, EventArgs e) {
            _player.MoveEast();
        }

        private void BtnWest_Click(object sender, EventArgs e) {
            _player.MoveWest();
        }

        private void BtnSouth_Click(object sender, EventArgs e) {
            _player.MoveSouth();
        }

        

        private void BtnUseWeapon_Click(object sender, EventArgs e) {
            Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;

            _player.UseWeapon(currentWeapon);
        }

        private void BtnUsePotion_Click(object sender, EventArgs e) {
            HealingPotion potion = (HealingPotion)cboPotions.SelectedItem;

            _player.UsePotion(potion);
        }

        private void RtbMessages_TextChanged(object sender, EventArgs e) {
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }

        private void SuperAdventure_FormClosing(object sender, FormClosingEventArgs e) {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, _player.ToXmlString());

            //PlayerDataMapper.SaveToDatabase(_player);
        }

        private void cboWeapons_SelectedIndexChanged(object sender, EventArgs e) {
            _player.CurrentWeapon = (Weapon)cboWeapons.SelectedItem;
        }

        private void cboArmors_SelectedIndexChanged(object sender, EventArgs e) {
            _player.CurrentArmor = (Armor)cboArmors.SelectedItem;
            lblDefenseVal.Text = _player.CurrentArmor.Defense.ToString();
        }

        private void btnTrade_Click(object sender, EventArgs e) {
            TradingScreen tradingScreen = new TradingScreen(_player);
            tradingScreen.StartPosition = FormStartPosition.CenterParent;
            tradingScreen.ShowDialog(this);
        }
    }
}
