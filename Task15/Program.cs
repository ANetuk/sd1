namespace Task15;

public static class QuickSorter
{
    /*
     * start >= 0 && end > start && end < arr.Length
     */
    private static int Partition(int[] arr, int start, int end)
    {
        var pivot = arr[end];

        var i = start;
        var j = start;

        while (j < end)
        /*
         * multiset(arr[..]) == multiset(old(arr[..]))
         * startIndex <= i <= j <= endIndex
         * forall k :: startIndex <= k < i ==> arr[k] <= pivot
         * forall k :: i <= k < j ==> arr[k] >= pivot
         */
        {
            if (arr[j] <= pivot)
            {
                (arr[i], arr[j]) = (arr[j], arr[i]);
                i += 1;
            }

            j += 1;
        }

        (arr[i], arr[end]) = (arr[end], arr[i]);
        return i;
    }
    /*
     * multiset(arr[..]) == multiset(old(arr[..]))
     * pivotIndex >= startIndex && pivotIndex <= endIndex
     * forall k :: startIndex <= k < pivotIndex ==> arr[k] <= arr[pivotIndex]
     * forall k :: pivotIndex < k <= endIndex ==> arr[k] >= arr[pivotIndex]
     */

    /*
     * start >= 0 && start < arr.Length && end >= start && end < arr.Length
     */
    public static void QuickSort(int[] arr, int start, int end)
    {
        if (start >= end) return;
        var pivotIndex = Partition(arr, start, end);
        QuickSort(arr, start, pivotIndex - 1);
        QuickSort(arr, pivotIndex + 1, end);
    }
    /*
     * multiset(arr[..]) == multiset(old(arr[..]))
     * forall k :: startIndex < k <= endIndex ==> arr[k] >= arr[k-1]
     */

    /*
     * Функция QuickSort имеет постусловие, что массив отсортирован в рамках указанных индексов.
     * При условии start >= end получается, что указанная часть состоит из одного элемента.
     * Такая часть, исходя из условия, является отсортированной.
     * В ином случае указанная часть массива изменяется таким образом, что в ней выделяется опорный элемент,
     * слева от которого находятся элементы меньше или равные опорному, а справа элементы большие или равные опорному.
     * Функция Partition включает соответствующую логику и постусловие.
     * Затем левые и правые части рекурсивно сортируются функцией QuickSort, и исходя из постусловий Partition и
     * QuickSort, получится, что массив в рамках исходных индексов будет отсортирован.
     * В функции Partition цикл включает инвариант startIndex <= k < i ==> arr[k] <= pivot,
     * который обозначает левую часть, и для правой части forall k :: i <= k < j ==> arr[k] >= pivot.
     * Каждую итерацию проверяется j элемент, и если он меньше или равен опорному, то он перемещается на позицию i,
     * после чего i увеличивается. Таким образом инвариант для левой части будет сохраняться.
     * При этом так как изначально j = start и i = start, и исходя из условия перестановки, инвариант для правой части
     * так же будет сохраняться.
     * После выполнения цикла получается, что i элемент больше или равен опорному элементу.
     * Следовательно, выполняется перестановка i элемента и опорного элемента. После чего исходя из инварианта и
     * выполненной перестановки станет выполняться постусловие функции для левой части, опорного элемента и правой части.
     */
}
