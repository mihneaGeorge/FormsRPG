﻿using Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsRPG {
    public partial class TradingScreen : Form {
        public Player _currentPlayer;
        public static int counter = 0;
        public TradingScreen(Player player) {
            _currentPlayer = player;

            InitializeComponent();

            DataGridViewCellStyle rightAlignedCellStyle = new DataGridViewCellStyle();
            rightAlignedCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvMyItems.RowHeadersVisible = false;
            dgvMyItems.AutoGenerateColumns = false;
            lblVendorInventory.Text = _currentPlayer.CurrentLocation.VendorWorkingHere.Name + "'s inventory";

            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn {
                DataPropertyName = "ItemID",
                Visible = false
            });

            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Name",
                Width = 100,
                DataPropertyName = "Description"
            });

            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Qty",
                Width = 30,
                DefaultCellStyle = rightAlignedCellStyle,
                DataPropertyName = "Quantity"
            });

            dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Price",
                Width = 35,
                DefaultCellStyle = rightAlignedCellStyle,
                DataPropertyName = "Price"
            });

            dgvMyItems.Columns.Add(new DataGridViewButtonColumn {
                Text = "Sell 1",
                UseColumnTextForButtonValue = true,
                Width = 50,
                DataPropertyName = "ItemID"
            });

            dgvMyItems.DataSource = _currentPlayer.Inventory;

            dgvMyItems.CellClick += dgvMyItems_CellClick;


            dgvVendorItems.RowHeadersVisible = false;
            dgvVendorItems.AutoGenerateColumns = false;

            dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn {
                DataPropertyName = "ItemID",
                Visible = false
            });

            dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Name",
                Width = 100,
                DataPropertyName = "Description"
            });

            dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn {
                HeaderText = "Price",
                Width = 35,
                DefaultCellStyle = rightAlignedCellStyle,
                DataPropertyName = "Price"
            });

            dgvVendorItems.Columns.Add(new DataGridViewButtonColumn {
                Text = "Buy 1",
                UseColumnTextForButtonValue = true,
                Width = 50,
                DataPropertyName = "ItemID"
            });

            dgvVendorItems.DataSource = _currentPlayer.CurrentLocation.VendorWorkingHere.Inventory;

            dgvVendorItems.CellClick += dgvVendorItems_CellClick;




        }

        private void BtnClose_Click(object sender, EventArgs e) {
            Close();
        }

        private void dgvMyItems_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (counter % 2 == 0) {
                if (e.ColumnIndex == 4) {
                    var itemID = dgvMyItems.Rows[e.RowIndex].Cells[0].Value;

                    Item itemBeingSold = World.ItemByID(Convert.ToInt32(itemID));

                    if (itemBeingSold.Price == World.UNSELLABLE_ITEM_PRICE) {
                        MessageBox.Show("You cannot sell the " + itemBeingSold.Name);
                    } else {
                        _currentPlayer.RemoveItemFromInventory(itemBeingSold);

                        _currentPlayer.Gold += itemBeingSold.Price;
                    }
                }
            }
            counter++;
        }

        private void dgvVendorItems_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (counter % 2 == 0) {
                if (e.ColumnIndex == 3) {
                    var itemID = dgvVendorItems.Rows[e.RowIndex].Cells[0].Value;

                    Item itemBeingBought = World.ItemByID(Convert.ToInt32(itemID));

                    if (_currentPlayer.Gold >= itemBeingBought.Price) {
                        _currentPlayer.AddItemToInventory(itemBeingBought);

                        _currentPlayer.Gold -= itemBeingBought.Price;
                    } else {
                        MessageBox.Show("You do not have enough gold to buy the " + itemBeingBought.Name);
                    }
                }
            }
            counter++;
        }
    
    }
}
