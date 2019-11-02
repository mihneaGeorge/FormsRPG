//using Npgsql;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Engine {
//    public static class PlayerDataMapper {
//        private static readonly string _connectionString = "User ID=postgres;Password=;Host=127.0.0.1;Port=5432;Database=SuperAdventure; Pooling=true;";
        
//        public static Player CreateFromDatabase() {
//            try {
//                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString)) {
//                    connection.Open();

//                    Player player;

//                    using(NpgsqlCommand savedGameCommand = connection.CreateCommand()) {
//                        savedGameCommand.CommandType = CommandType.Text;
//                        savedGameCommand.CommandText = "SELECT * FROM SavedGame";

//                        NpgsqlDataReader reader = savedGameCommand.ExecuteReader();

//                        if (!reader.HasRows) {
//                            return null;
//                        }

//                        reader.Read();

//                        int currentHitPoints = (int)reader["CurrentHitPoints"];
//                        int maximumHitPoints = (int)reader["MaximumHitPoints"];
//                        int gold = (int)reader["Gold"];
//                        int experiencePoints = (int)reader["ExperiencePoints"];
//                        int currentLocationID = (int)reader["CurrentLocationID"];

//                        player = Player.CreatePlayerFromDatabase(currentHitPoints, maximumHitPoints, gold, experiencePoints, currentLocationID);

//                        reader.Close();
//                    }

//                    using(NpgsqlCommand questCommand = connection.CreateCommand()) {
//                        questCommand.CommandType = CommandType.Text;
//                        questCommand.CommandText = "SELECT * FROM Quest";

//                        NpgsqlDataReader reader = questCommand.ExecuteReader();

//                        if (reader.HasRows) {
//                            while (reader.Read()) {
//                                int questID = (int)reader["QuestID"];
//                                bool isCompleted = (bool)reader["IsCompleted"];

//                                PlayerQuest playerQuest = new PlayerQuest(World.QuestByID(questID));
//                                playerQuest.IsCompleted = isCompleted;

//                                player.Quests.Add(playerQuest);
//                            }
//                        }
//                        reader.Close();
//                    }

//                    using(NpgsqlCommand inventoryCommand = connection.CreateCommand()) {
//                        inventoryCommand.CommandType = CommandType.Text;
//                        inventoryCommand.CommandText = "SELECT * FROM public.inventory";

//                        NpgsqlDataReader reader = inventoryCommand.ExecuteReader();

//                        if (reader.HasRows) {
//                            while (reader.Read()) {
//                                int inventoryItemID = Convert.ToInt32(reader["invetoryitemid"]);
//                                int quantity = (int)reader["quantity"];

//                                player.AddItemToInventory(World.ItemByID(inventoryItemID), quantity);
//                            }
//                        }
//                    }
//                    return player;
//                }
//            } catch (Exception ex) {

//            }
//            return null;
//        }

//        //public static void SaveToDatabase(Player player) {
//        //    try {
//        //        using(NpgsqlConnection connection = new NpgsqlConnection(_connectionString)) {
//        //            connection.Open();
//        //            using (NpgsqlCommand existingRowCountCommand = connection.CreateCommand()) {
//        //                existingRowCountCommand.CommandType = CommandType.Text;
//        //                existingRowCountCommand.CommandText = "SELECT COUNT(*) FROM savedgame";

//        //                int existingRowCount = Convert.ToInt32(existingRowCountCommand.ExecuteScalar());

//        //                if (existingRowCount == 0) {
//        //                    using(NpgsqlCommand insertSavedGame = connection.CreateCommand()) {
//        //                        insertSavedGame.CommandType = CommandType.Text;
//        //                        insertSavedGame.CommandText =
//        //                            "INSERT INTO SavedGame " +
//        //                            "(CurrentHitPoints, MaximumHitPoints, Gold, ExperiencePoints, CurrentLocationID) " +
//        //                            "VALUES " +
//        //                            "(@CurrentHitPoints, @MaximumHitPoints, @Gold, @ExperiencePoints, @CurrentLocationID)";

//        //                        insertSavedGame.Parameters.Add("@CurrentHitPoints", NpgsqlTypes.NpgsqlDbType.Integer);
//        //                        insertSavedGame.Parameters["@CurrentHitPoints"].Value = player.CurrentHitPoints;

//        //                        insertSavedGame.Parameters.Add("@MaximumHitPoints", NpgsqlTypes.NpgsqlDbType.Integer);
//        //                        insertSavedGame.Parameters["@MaximumHitPoints"].Value = player.MaximumHitPoints;

//        //                        insertSavedGame.Parameters.Add("@Gold", NpgsqlTypes.NpgsqlDbType.Integer);
//        //                        insertSavedGame.Parameters["@Gold"].Value = player.Gold;

//        //                        insertSavedGame.Parameters.Add("@ExperiencePoints", NpgsqlTypes.NpgsqlDbType.Integer);
//        //                        insertSavedGame.Parameters["@ExperiencePoints"].Value = player.ExperiencePoints;

//        //                        insertSavedGame.Parameters.Add("@CurrentLocationID", NpgsqlTypes.NpgsqlDbType.Integer);
//        //                        insertSavedGame.Parameters["@CurrentLocationID"].Value = player.CurrentLocation.ID;

//        //                        insertSavedGame.ExecuteNonQuery();
//        //                    }
//        //                } else {
//        //                    using(NpgsqlCommand updateSavedGame = connection.CreateCommand()) {
//        //                        updateSavedGame.CommandType = CommandType.Text;
//        //                        updateSavedGame.CommandText =
//        //                            "UPDATE SavedGame " +
//        //                            "SET CurrentHitPoints = @CurrentHitPoints, " +
//        //                            "MaximumHitPoints = @MaximumHitPoints," +
//        //                            "Gold = @Gold," +
//        //                            "ExperiencePoints = @ExperiencePoints," +
//        //                            "CurrentLocationID = @CurrentLocationID";

//        //                        updateSavedGame.Parameters.Add("@CurrentHitPoints", NpgsqlTypes.NpgsqlDbType.Integer);
//        //                        updateSavedGame.Parameters["@CurrentHitPoints"].Value = player.CurrentHitPoints;

//        //                        updateSavedGame.Parameters.Add("@MaximumHitPoints", NpgsqlTypes.NpgsqlDbType.Integer);
//        //                        updateSavedGame.Parameters["@MaximumHitPoints"].Value = player.MaximumHitPoints;

//        //                        updateSavedGame.Parameters.Add("@Gold", NpgsqlTypes.NpgsqlDbType.Integer);
//        //                        updateSavedGame.Parameters["@Gold"].Value = player.Gold;

//        //                        updateSavedGame.Parameters.Add("@ExperiencePoints", NpgsqlTypes.NpgsqlDbType.Integer);
//        //                        updateSavedGame.Parameters["@ExperiencePoints"].Value = player.ExperiencePoints;

//        //                        updateSavedGame.Parameters.Add("@CurrentLocationID", NpgsqlTypes.NpgsqlDbType.Integer);
//        //                        updateSavedGame.Parameters["@CurrentLocationID"].Value = player.CurrentLocation.ID;

//        //                        updateSavedGame.ExecuteNonQuery();
//        //                    }
//        //                }
//        //            }

//        //            using(NpgsqlCommand deleteQuestsCommand = connection.CreateCommand()) {
//        //                deleteQuestsCommand.CommandType = CommandType.Text;
//        //                deleteQuestsCommand.CommandText = "DELETE FROM Quest";

//        //                deleteQuestsCommand.ExecuteNonQuery();
//        //            }

//        //            foreach (PlayerQuest playerQuest in player.Quests) {
//        //                using(NpgsqlCommand insertQuestCommand = connection.CreateCommand()) {
//        //                    insertQuestCommand.CommandType = CommandType.Text;
//        //                    insertQuestCommand.CommandText =
//        //                        "INSERT INTO Quest (QuestID, IsCompleted) VALUES (@QuestID, @IsCompleted)";

//        //                    insertQuestCommand.Parameters.Add("@QuestID", NpgsqlTypes.NpgsqlDbType.Integer);
//        //                    insertQuestCommand.Parameters["@QuestID"].Value = playerQuest.Details.ID;

//        //                    insertQuestCommand.Parameters.Add("@IsCompleted", NpgsqlTypes.NpgsqlDbType.Bit);
//        //                    insertQuestCommand.Parameters["@IsCompleted"].Value = playerQuest.IsCompleted;

//        //                    insertQuestCommand.ExecuteNonQuery();
//        //                }
//        //            }

//        //            using(NpgsqlCommand deleteInventoryCommand = connection.CreateCommand()) {
//        //                deleteInventoryCommand.CommandType = CommandType.Text;
//        //                deleteInventoryCommand.CommandText = "DELETE FROM Inventory";

//        //                deleteInventoryCommand.ExecuteNonQuery();
//        //            }

//        //            foreach(InventoryItem inventoryItem in player.Inventory) {
//        //                using(NpgsqlCommand insertInventoryCommand = connection.CreateCommand()) {
//        //                    insertInventoryCommand.CommandType = CommandType.Text;
//        //                    insertInventoryCommand.CommandText = "INSERT INTO Inventory (invetoryitemid, quantity) VALUES (@invetoryitemid, @quantity)";

//        //                    insertInventoryCommand.Parameters.Add("@invetoryitemid", NpgsqlTypes.NpgsqlDbType.Integer);
//        //                    insertInventoryCommand.Parameters["@invetoryitemid"].Value = inventoryItem.Details.ID;

//        //                    insertInventoryCommand.Parameters.Add("@quantity", NpgsqlTypes.NpgsqlDbType.Integer);
//        //                    insertInventoryCommand.Parameters["@quantity"].Value = inventoryItem.Quantity;

//        //                    insertInventoryCommand.ExecuteNonQuery();
//        //                }
//        //            }
//        //        }
//        //    } catch (Exception ex) {

//        //    }
//        //}
//    }
//}
