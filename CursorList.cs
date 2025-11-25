namespace CursorListNamespace
{
    public struct PersonData
    {
        private char[] Name;
        private char[] Address;

        public PersonData(char[] name, char[] address)
        {
            Name = new char[20];
            Address = new char[50];

            Array.Copy(name, Name, Math.Min(20, name.Length));
            Array.Copy(address, Address, Math.Min(50, address.Length));
        }

        public override bool Equals(object obj)
        {
            if (obj is PersonData other)
            {
                return new string(this.Name) == new string(other.Name) &&
                       new string(this.Address) == new string(other.Address);
            }
            return false;
        }

        public override string ToString()
        {
            string nameStr = new string(Name).TrimEnd('\0');
            string addressStr = new string(Address).TrimEnd('\0');
            return $"Name: {nameStr}, Address: {addressStr}";
        }
    }

    public class Node<T>
    {
        private T data;
        private int next;

        public Node()
        {
            this.next = -1;
        }

        public Node(T data)
        {
            this.data = data;
            this.next = -1;
        }

        public T getData()
        {
            return this.data;
        }

        public int getNext()
        {
            return this.next;
        }

        public void setNext(int next)
        {
            this.next = next;
        }

        public void setData(T data)
        {
            this.data = data;
        }


    }

    public class Position<T>
    {
        private int index;
        public Position(int index)
        {
            this.index = index;
        }

        public int getPosition()
        {
            return index;
        }

        public void setPosition(int value)
        {
            this.index = value;
        }
    }

    public class List<T>
    {
        private int start;
        private int space;
        private Node<T>[] list;
        private int SIZE = 50;

        public List()
        {
            list = new Node<T>[this.SIZE];

            for (int i = 0; i < this.SIZE; i++)
            {
                list[i] = new Node<T>();
                list[i].setNext(i + 1);
            }
            list[this.SIZE - 1].setNext(-1);
            this.start = -1;
            this.space = 0;
        }

        public Position<T> End()
        {
            return new Position<T>(-1);
        }

        public void Insert(Position<T> p, T x)
        {
            // Если вставляемая позиция - это позиция после последнего
            if (p.getPosition() == -1)
            {
                // Если есть свободная ячейка, вставляем в нее
                if (this.space != -1)
                {
                    // Сохраняем номер ячейки, которая сейчас первая свободная
                    int nodeToInsert = this.space;
                    // В space сохраняем номер следующей свободной ячейки
                    this.space = this.list[this.space].getNext();
                    // Сохраняем в ячейку передаваемое значение
                    this.list[nodeToInsert].setData(x);
                    this.list[nodeToInsert].setNext(-1);
                    // Если при этом список был пустой, сохраняем в Start ячейку
                    if (this.start == -1)
                    {
                        this.start = nodeToInsert;
                    }
                    // Если нет, то нужно пройтись по элементам, найти тот, который ведет на -1, записать туда номер текущей ячейки
                    else
                    {
                        int currentNode = this.start;
                        while (this.list[currentNode].getNext() != -1)
                        {
                            currentNode = this.list[currentNode].getNext();
                        }

                        // Когда нашли, теперь этот элемент ведет в тот, куда вставляем
                        this.list[currentNode].setNext(nodeToInsert);
                    }

                }


            }
            // Если вставляем не в позицию после последнего
            else
            {
                // Будем вставлять только если в списке есть свободное место
                if (this.space != -1)
                {
                    int nodeToInsert = p.getPosition();
                    // пойдем с головы по элементам списка, чтобы проверить, есть ли вообще такая позиция в списке
                    int currentNode = this.start;
                    while (currentNode != -1)
                    {
                        // Если нашли такой элемент
                        if (currentNode == nodeToInsert)
                        {
                            // В пустую ячейку вставляем то, что раньше было во вставляемой
                            this.list[this.space].setData(this.list[nodeToInsert].getData());
                            // Следующая для вставляемой ячейки будет space
                            this.list[nodeToInsert].setNext(this.space);
                            // Теперь пустая ячейка - та, на которую укзаывает пустая
                            this.space = this.list[this.space].getNext();
                            // Записываем собственно данные в ячейку p
                            this.list[nodeToInsert].setData(x);

                            // Если вставляемая позиция раньше была головой, нужно поменять голову
                            if (nodeToInsert == this.start)
                            {
                                this.start = this.list[nodeToInsert].getNext();
                            }

                            // После чего можно дальше не идти
                            break;
                        }
                        // Переходим к слеждующему элементу
                        currentNode = this.list[currentNode].getNext();
                    }


                }

            }
        }

        public Position<T> Locate(T x)
        {
            // Проходим от начала по элементам, если нашли нужный, выводим позицию
            int currentNode = this.start;

            while (currentNode != -1)
            {
                if (this.list[currentNode].getData().Equals(x))
                {
                    return new Position<T>(currentNode);
                }

                currentNode = this.list[currentNode].getNext();
            }

            return End();
        }

        public T Retrieve(Position<T> p)
        {
            int currentNode = this.start;

            while (currentNode != -1)
            {
                if (currentNode == p.getPosition())
                {
                    return this.list[currentNode].getData();
                }
                currentNode = this.list[currentNode].getNext();
            }

            return default(T);

        }

        public void Delete(Position<T> p)
        {
            // Если мы удаляем start
            if (this.start == p.getPosition())
            {
                int prevStart = this.start;

                // Записываем в start следующий для start элемент
                this.start = this.list[this.start].getNext();

                // там где раньше был start теперь пустая ячейка, которая ведет в предыдущий Space
                this.list[prevStart].setNext(this.space);
                // Теперь space - это то, что было стартом
                this.space = prevStart;
            }
            // Если же мы удаляем не старт,пойдем по элементам, пока следующий не станет удаляемым
            else
            {
                // Начнем со старта
                int currentElement = this.start;

                // Будем рассматривать элементы, следующий для текущего
                // Пока элемент, следующий для текущего не равен -1
                while (this.list[currentElement].getNext() != -1)
                {
                    // Если элемент, следующий для текущего - удаляемый
                    if (this.list[currentElement].getNext() == p.getPosition())
                    {
                        // Сохраним удаляемый элемент
                        int deleteNode = p.getPosition();

                        // Теперь предыдущий для него элемент должен вести в следующий для него элемент
                        this.list[currentElement].setNext(this.list[deleteNode].getNext());

                        //Сам удаляемвй элемент теперь должен вести в Space
                        this.list[deleteNode].setNext(this.space);

                        // Теперь позиция удаленного элемента - это space
                        this.space = deleteNode;
                        return;
                    }
                    currentElement = this.list[currentElement].getNext();
                }
            }
        }

        public Position<T> Next(Position<T> p)
        {
            int currentNode = this.start;
            while (currentNode != -1)
            {
                if (currentNode == p.getPosition())
                {
                    if (this.list[currentNode].getNext() == -1)
                    {
                        return End();
                    }
                    return new Position<T>(this.list[currentNode].getNext());
                }
                currentNode = this.list[currentNode].getNext();
            }
            return null;
        }

        public Position<T> Previous(Position<T> p)
        {
            // Если вводимая позиция - не старт
            if (p.getPosition() != this.start)
            {
                int currentItem = this.start;
                // Пока элемент, следующий за текущим не будет -1
                while (this.list[currentItem].getNext() != -1)
                {
                    // Если элемент, следующий за текущим - искомый, то возвращаем текущий
                    if (this.list[currentItem].getNext() == p.getPosition())
                    {
                        return new Position<T>(currentItem);
                    }
                    currentItem = this.list[currentItem].getNext();
                }
            }

            // Если так ничего и не вернули
            return null;
        }

        public Position<T> makeNull()
        {
            // Теперь последний элемент списка должен вести в space
            int currentItem = this.start;

            // Доходим до последнего элемента списка
            while (this.list[currentItem].getNext() != -1)
            {
                currentItem = this.list[currentItem].getNext();
            }

            // Он теперь ведет в space
            this.list[currentItem].setNext(this.space);

            // Space теперь то, что было start

            this.space = this.start;
            this.start = -1;

            return End();
        }

        public Position<T> First()
        {
            return (this.start == -1 ? End() : new Position<T>(this.start));
        }

        public void PrintList()
        {
            int currentNode = this.start;

            while (currentNode != -1)
            {
                Console.WriteLine(this.list[currentNode].getData());
                currentNode = this.list[currentNode].getNext();
            }
        }
    }

}