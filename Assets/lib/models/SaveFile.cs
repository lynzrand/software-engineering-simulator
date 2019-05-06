using System;

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

        public SaveMetadata Metadata
        {
            get => new SaveMetadata
            {
                version = this.version,
                id = this.id,
                name = this.name,
                ut = this.company.time,
                fund = this.company.fund,
                reputation = this.company.reputation,
                employeeCount = this.company.employees.Count,
                taskCount = this.company.tasks.Count
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
        public int taskCount;
    }
}