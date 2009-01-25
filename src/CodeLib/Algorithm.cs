using System;
using System.Collections;
using System.Collections.Generic;

namespace QiHe.CodeLib
{
    /// <summary>
    /// The Algorithm object is used to perform the calculating tasks.
    /// </summary>
    public class Algorithm
    {
        public static byte[] ArraySection(byte[] data, int index, int count)
        {
            byte[] section = new byte[count];
            Array.Copy(data, index, section, 0, count);
            return section;
        }
        /// <summary>
        /// Compares two arrays
        /// </summary>
        /// <param name="bytes">The Array</param>
        /// <param name="data">The Array</param>
        /// <returns></returns>
        public static bool ArrayEqual(byte[] bytes, byte[] data)
        {
            if (bytes.Length == data.Length)
            {
                return ArrayEqual(bytes, data, bytes.Length);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Compares two arrays
        /// </summary>
        /// <param name="a">The Array</param>
        /// <param name="b">The Array</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static bool ArrayEqual(byte[] a, byte[] b, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Sets the indice on an array
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static int[] ArrayIndice(int length)
        {
            int[] indice = new int[length];
            for (int i = 0; i < length; i++)
            {
                indice[i] = i;
            }
            return indice;
        }
        /// <summary>
        /// Looks for the maximum length of the ArrayList
        /// </summary>
        /// <param name="list">list of lists</param>
        /// <returns>the maximum length</returns>
        public static int Maximum(ArrayList list)
        {
            int max = 0;
            foreach (ICollection sublist in list)
            {
                if (max < sublist.Count)
                {
                    max = sublist.Count;
                }
            }
            return max;
        }
        /// <summary>
        /// Sums the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static int Sum(IList<int> list)
        {
            int sum = 0;
            foreach (int n in list)
            {
                sum += n;
            }
            return sum;
        }

        /// <summary>
        /// Copies the array.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="dest">The dest.</param>
        /// <param name="start">The start.</param>
        public static void CopyArray(byte[] src, byte[] dest, int start)
        {
            int j = 0;
            for (int i = start; i < start + dest.Length; i++)
            {
                dest[j++] = src[i];
            }
        }


        /// <summary>
        /// Maximums the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static int Maximum(IEnumerable<int> list)
        {
            int max = int.MinValue;
            foreach (int num in list)
            {
                if (max < num)
                {
                    max = num;
                }
            }
            return max;
        }

        /// <summary>
        /// Minimums the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static int Minimum(IEnumerable<int> list)
        {
            int min = int.MaxValue;
            foreach (int num in list)
            {
                if (min > num)
                {
                    min = num;
                }
            }
            return min;
        }

        /// <summary>
        /// Minimums the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static uint Minimum(IEnumerable<uint> list)
        {
            uint min = uint.MaxValue;
            foreach (uint num in list)
            {
                if (min > num)
                {
                    min = num;
                }
            }
            return min;
        }

        /// <summary>
        /// Return the postion of the maximum in the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static int PosofMaximum(IList<int> list)
        {
            int index = 0;
            int max = list[0];
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] > max)
                {
                    max = list[i];
                    index = i;
                }
            }
            return index;
        }

        /// <summary>
        /// Return the postion of the Minimum in the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static int PosofMinimum(IList<int> list)
        {
            int index = 0;
            int min = list[0];
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] < min)
                {
                    min = list[i];
                    index = i;
                }
            }
            return index;
        }

        /// <summary>
        /// Return the postion of the Minimum in the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static int PosofMinimum(int[] list)
        {
            int index = 0;
            int min = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                if (list[i] < min)
                {
                    min = list[i];
                    index = i;
                }
            }
            return index;
        }


        /// <summary>
        /// Return the postion of the Maximum in the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static List<int> PosofMaximums(IList<int> list)
        {
            int max = int.MinValue;
            List<int> maxIndice = new List<int>();
            for (int index = 0; index < list.Count; index++)
            {
                int num = list[index];
                if (num > max)
                {
                    max = num;
                    maxIndice.Clear();
                }
                if (num == max)
                {
                    maxIndice.Add(index);
                }
            }
            return maxIndice;
        }

        /// <summary>
        /// Return the postion of the Minimum in the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static List<int> PosofMinimums(IList<int> list)
        {
            int min = int.MaxValue;
            List<int> minIndice = new List<int>();
            for (int index = 0; index < list.Count; index++)
            {
                int num = list[index];
                if (num < min)
                {
                    min = num;
                    minIndice.Clear();
                }
                if (num == min)
                {
                    minIndice.Add(index);
                }
            }
            return minIndice;
        }

        /// <summary>
        /// Flats the specified groups.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        public static List<T> Flat<T>(List<List<T>> groups)
        {
            List<T> items = new List<T>();
            foreach (List<T> group in groups)
            {
                items.AddRange(group);
            }
            return items;
        }
    }
}
