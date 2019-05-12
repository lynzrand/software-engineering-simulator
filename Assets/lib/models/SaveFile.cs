using System;
using System.Collections.Generic;
using Ceras;

namespace Sesim.Models
{
    public class SaveFile
    {
        public long version = 0;

        /// <summary>
        /// The ID of the savefile, should not be modified
        /// </summary>
        public Ulid id;

        public string name;

        public Company company;

        public DifficultySettings settings;

        [Exclude]
        public SaveMetadata Metadata
        {
            get => new SaveMetadata
            {
                version = this.version,
                id = this.id,
                name = this.name,
                ut = this.company.ut,
                fund = this.company.fund,
                reputation = this.company.reputation,
                employeeCount = this.company.employees.Count,
                contractCount = this.company.contracts.Count
            };
        }
    }

    public class SaveMetadata
    {
        public long version = 0;
        public Ulid id;
        public string name;
        public int ut;
        public decimal fund;
        public float reputation;
        public int employeeCount;
        public int contractCount;

        public SaveMetadata() { }

        public SaveMetadata(long version, Ulid id, string name, int ut, decimal fund, float reputation, int employeeCount, int contractCount)
        {
            this.version = version;
            this.id = id;
            this.name = name;
            this.ut = ut;
            this.fund = fund;
            this.reputation = reputation;
            this.employeeCount = employeeCount;
            this.contractCount = contractCount;
        }

        public override string ToString(){
            return $"Savefile(version: {version}, id: {id}, name: \"{name}\", ut: {ut}, fund: {fund}, reputation: {reputation}, employeeCount: {employeeCount}, contractCount: {contractCount})";
        }
    }
}
