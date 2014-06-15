using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace EngineeringTemplateMaster
{
    //rjudge: TODO:
    //8 units of coolant is generally all you have to distribute.
    //  - Allow for distributing more, but with warnings
	public class EngineeringPreset
	{
		private int[] _energyLevel;
		private byte[] _coolantAmount;

		private EngineeringSettings _engineeringSettings;

		public int GetEnergyLevel(int systemID)
		{
			return _energyLevel[systemID];
		}

		public void SetEnergyLevel(int systemID, int level)
		{
            if (level > 300) level = 300;
            if (level < 0) level = 0;
            if (level > 76 && level < 124) level = 100;
            if (_energyLevel[systemID] == level)
				return;
			_energyLevel[systemID] = level;
			_engineeringSettings.ChangesPending = true;
		}
		
		public byte GetCoolantAmount(int systemID)
		{
			return _coolantAmount[systemID];
		}

		public void SetCoolantAmount(int systemID, byte amount)
		{
			if (amount > 8) amount = 8;
			if (_coolantAmount[systemID] == amount)
				return;
			_coolantAmount[systemID] = amount;
			_engineeringSettings.ChangesPending = true;
		}

        public void ReadEnergyFromFile(int systemID, BinaryReader br)
		{
			_energyLevel[systemID] = (int)Math.Round(br.ReadSingle() * 300);
		}

		public void WriteEnergyToFile(int systemID, BinaryWriter bw)
		{
			bw.Write((float)_energyLevel[systemID]/300);
		}

		public void ReadCoolantFromFile(int systemID, BinaryReader br)
		{
			_coolantAmount[systemID] = br.ReadByte();
		}

		public void WriteCoolantToFile(int systemID, BinaryWriter bw)
		{
			bw.Write(_coolantAmount[systemID]);
		}

		public void ResetEnergy(int value = 100)
		{
			for (int i = 0; i < 8; i++)
				SetEnergyLevel(i, value);
		}

		public void ResetCoolant()
		{
			for (int i = 0; i < 8; i++)
				SetCoolantAmount(i, 0);
		}

        public static double GetCoolantNeed(double level)
        {
            if (level <= 100.0)
                return 0.0;
            else if (level <= 150.0)
                return 0.0 + (level - 100.0) / 25.0;
            else if (level <= 190.0)
                return 2.0 + (level - 150.0) / 20.0;
            else if (level <= 220.0)
                return 4.0 + (level - 190.0) / 15.0;
            else if (level <= 250.0)
                return 6.0 + (level - 220.0) / 15.0;
            else
                return 8.0;
        }

        public void AutoCoolant(int max)
        {
            double[] coolantNeed = new double[8];
            int totalNeed = 0;

            for (int i = 0; i < 8; i++)
            {
                SetCoolantAmount(i, 0);

                coolantNeed[i] = GetCoolantNeed(GetEnergyLevel(i));

                totalNeed += (int)Math.Ceiling(coolantNeed[i]);
            }

            if (totalNeed <= max)
            {
                for (int i = 0; i < 8; i++)
                    SetCoolantAmount(i, (byte)Math.Ceiling(coolantNeed[i]));
                return;
            }

            while (max-- > 0)
            {
                int id = -1;
                double diff = 0;
                for (int i = 0; i < 8; i++)
                    if (diff < coolantNeed[i] - GetCoolantAmount(i))
                    {
                        diff = coolantNeed[i] - GetCoolantAmount(i);
                        id = i;
                    }
                SetCoolantAmount(id, (byte)(GetCoolantAmount(id) + 1));
            }
        }

        public void CapCoolant(int max)
        {
            double[] coolantNeed = new double[8];
            int totalUsed = 0;

            for (int i = 0; i < 8; i++)
            {
                coolantNeed[i] = GetCoolantNeed(GetEnergyLevel(i));
                totalUsed += GetCoolantAmount(i);
            }

            if (totalUsed <= max)
                return;

            while (totalUsed-- > max)
            {
                int id = -1;
                double diff = -8;
                for (int i = 0; i < 8; i++)
                    if (GetCoolantAmount(i) > 0 && diff < GetCoolantAmount(i) - coolantNeed[i])
                    {
                        diff = GetCoolantAmount(i) - coolantNeed[i];
                        id = i;
                    }
                SetCoolantAmount(id, (byte)(GetCoolantAmount(id) - 1));
            }
        }
        		
		public EngineeringPreset(EngineeringSettings es)
		{
			_energyLevel = new int[8] { 100, 100, 100, 100, 100, 100, 100, 100 };
			_coolantAmount = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
			_engineeringSettings = es;
		}
		
	}

	public class EngineeringSettings
	{
		public string Name { get; set; }

		public RefreshingListBox ListBox { get; set; }

		/// <summary> Flag that there are changes that need saving to file </summary>
		private bool _changesPending = false;
		/// <summary> Flag that there are changes that need saving to file </summary>
		public bool ChangesPending { get { return _changesPending; } set { if (_changesPending != value) _changesPending = value; UpdateFormText(); } }

		public EngineeringPreset[] Presets;

        public void CopyPreset(int sourceID, int targetID)
        {
            for (int j = 0; j < 8; j++)
            {
                Presets[targetID].SetEnergyLevel(j, Presets[sourceID].GetEnergyLevel(j));
                Presets[targetID].SetCoolantAmount(j, Presets[sourceID].GetCoolantAmount(j));
            }
        }

        public int GetMaxCoolant()
		{
			int result = 0;
			for (int i = 0; i < 10; i++)
			{
				int tmp = 0;
				for (int j = 0; j < 8; j++)
					tmp += Presets[i].GetCoolantAmount(j);
				result = Math.Max(result, tmp);
			}
			return result;
		}

		public override string ToString()
		{
			return "(" + GetMaxCoolant().ToString("00") +")" + " " + Name + (ChangesPending ? " <*>" : "");
		}

		public void UpdateFormText()
		{
			if (ListBox != null && ListBox.SelectedItem != null)
				ListBox.RefreshItems();
		}

		public bool LoadFromFile(string fileName)
		{
			bool result = true;

			FileStream fs;
			BinaryReader br;

			try 
			{
				fs = File.Open(fileName, FileMode.Open);
				br = new BinaryReader(fs);
			}
			catch//(Exception e)
			{
				//TODO: Handle error
				return false;
			}

			try
			{
				if (br.ReadByte() != (byte)254 || br.ReadByte() != (byte)254)
					throw new Exception("Header \"0xfefe\" missing!");

				for (int i = 1; i < 11; i++)
				{
                    for (int j = 0; j < 8; j++)
                        Presets[i % 10].ReadEnergyFromFile(j, br);
					for (int j = 0; j < 8; j++)
                        Presets[i % 10].ReadCoolantFromFile(j, br);
				}

				Name = Path.GetFileNameWithoutExtension(fileName);
				ChangesPending = false;

			}
			catch //(Exception e)
			{
				result = false;
				//TODO: Handle error
			}
			finally
			{
				fs.Close();
			}
			
			return result;
		}

		public bool SaveToFile(string fileName)
		{
			bool result = true;

			FileStream fs;
			BinaryWriter bw;

			try
			{
				fs = File.Open(fileName, FileMode.Create);
				bw = new BinaryWriter(fs);
			}
			catch
			{
				//TODO: Handle error
				return false;
			}

			try
			{
				//write header "fe fe"
				bw.Write((byte)254);
				bw.Write((byte)254);

				for (int i = 1; i < 11; i++)
				{
					for (int j = 0; j < 8; j++)
                        Presets[i % 10].WriteEnergyToFile(j, bw);
					for (int j = 0; j < 8; j++)
                        Presets[i % 10].WriteCoolantToFile(j, bw);
				}

			}
			catch //(Exception e)
			{
				result = false;
				//TODO: Handle error
			}
			finally
			{
				fs.Close();
			}

			return result;
		}

		public EngineeringSettings()
		{
			Presets = new EngineeringPreset[10] { new EngineeringPreset(this), new EngineeringPreset(this), new EngineeringPreset(this), new EngineeringPreset(this), new EngineeringPreset(this), new EngineeringPreset(this), new EngineeringPreset(this), new EngineeringPreset(this), new EngineeringPreset(this), new EngineeringPreset(this)};
			ListBox = null;
		}

		public EngineeringSettings(string fileName) : this()
		{
			LoadFromFile(fileName);
		}

	}
}
