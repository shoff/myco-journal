namespace Myco.Domain
{
    using System.Collections;
    using System.Collections.Generic;
    using ChaosMonkey.Guards;
    using Microsoft.Extensions.DependencyInjection;

    public class MycoServiceCollection : IServiceCollection
    {
        private readonly List<ServiceDescriptor> serviceDescriptors = new List<ServiceDescriptor>();

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return this.serviceDescriptors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ServiceDescriptor item)
        {
            Guard.IsNotNull(item, nameof(item));
            this.serviceDescriptors.Add(item);
        }

        public void Clear()
        {
            this.serviceDescriptors.Clear();
        }

        public bool Contains(ServiceDescriptor item)
        {
            Guard.IsNotNull(item, nameof(item));
            return this.serviceDescriptors.Contains(item);
        }

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            Guard.IsNotNull(array, nameof(array));
            this.serviceDescriptors.CopyTo(array, arrayIndex);
        }

        public bool Remove(ServiceDescriptor item)
        {
            Guard.IsNotNull(item, nameof(item));
            return this.serviceDescriptors.Remove(item);
        }

        public int Count => this.serviceDescriptors.Count;
        public bool IsReadOnly => false;
        public int IndexOf(ServiceDescriptor item)
        {
            Guard.IsNotNull(item, nameof(item));
            return this.serviceDescriptors.IndexOf(item);
        }

        public void Insert(int index, ServiceDescriptor item)
        {
            Guard.IsNotNull(item, nameof(item));
            this.serviceDescriptors.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            if (index > this.serviceDescriptors.Count)
            {
                return;
            }

            this.serviceDescriptors.RemoveAt(index);
        }


        public ServiceDescriptor this[int index]
        {
            get => this.serviceDescriptors[index];
            set
            {
                Guard.IsNotNull(value, nameof(value));
                this.serviceDescriptors[index] = value;
            }
        }
    }
}