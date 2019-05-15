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
        public double ut;
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

        public override bool Equals(object obj)
        {
            return obj is SaveMetadata metadata &&
                   version == metadata.version &&
                   id == metadata.id &&
                   name == metadata.name &&
                   ut == metadata.ut &&
                   fund == metadata.fund &&
                   reputation == metadata.reputation &&
                   employeeCount == metadata.employeeCount &&
                   contractCount == metadata.contractCount;
        }

        public override int GetHashCode()
        {
            var hashCode = 1756717635;
            hashCode = hashCode * -1521134295 + version.GetHashCode();
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + name.GetHashCode();
            hashCode = hashCode * -1521134295 + ut.GetHashCode();
            hashCode = hashCode * -1521134295 + fund.GetHashCode();
            hashCode = hashCode * -1521134295 + reputation.GetHashCode();
            hashCode = hashCode * -1521134295 + employeeCount.GetHashCode();
            hashCode = hashCode * -1521134295 + contractCount.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"Savefile(version: {version}, id: {id}, name: \"{name}\", ut: {ut}, fund: {fund}, reputation: {reputation}, employeeCount: {employeeCount}, contractCount: {contractCount})";
        }
    }
}
