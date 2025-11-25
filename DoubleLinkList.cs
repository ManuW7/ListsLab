namespace DoubleLinksListNamespace
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
            // Превращаем char[] в строку и убираем лишние нули
            string nameStr = new string(Name).TrimEnd('\0');
            string addressStr = new string(Address).TrimEnd('\0');
            return $"Name: {nameStr}, Address: {addressStr}";
        }
    }

    public class Node<T>
    {
        private T data { get; set; }
        private Node<T>? previous;
        private Node<T>? next;

        public Node(T data)
        {
            this.data = data;
            previous = null;
            next = null;
        }

        public T getData()
        {
            return this.data;
        }

        public Node<T>? getPrevious()
        {
            return previous;
        }

        public void setPrevious(Node<T>? previous)
        {
            this.previous = previous;
        }

        public Node<T>? getNext()
        {
            return next;
        }

        public void setNext(Node<T>? next)
        {
            this.next = next;
        }


    }

    public class Position<T>
    {
        private Node<T>? position;

        public Node<T>? getPosition()
        {
            return position;
        }

        public void setPosition(Node<T>? position)
        {
            this.position = position;
        }

        public Position(Node<T> position)
        {
            this.position = position;
        }
    }

    public class List<T>
    {

        private Node<T>? head;
        private Node<T>? tail;

        public List()
        {
            head = null;
            tail = null;
        }


        public Position<T> End()
        {
            return new Position<T>(null);
        }

        public void Insert(Position<T> p, T x)
        {
            Node<T> newNode = new Node<T>(x);


            // Если переданная позиция - это позиция после последнего
            if (p.getPosition() == null)
            {
                // Если при этом список пустой
                if (this.head == null)
                {
                    this.head = newNode;
                    this.tail = newNode;
                }
                // Если при этом список не пустой
                else
                {
                    newNode.setPrevious(this.tail);
                    this.tail.setNext(newNode);
                    this.tail = newNode;
                }
            }
            // Если переданная позиция - не позиция после последнего
            else
            {
                // Если вставляем в head
                if (p.getPosition() == head)
                {
                    this.head.setPrevious(newNode);
                    newNode.setNext(this.head);
                    this.head = newNode;
                }
                // Если вставляем в tail
                else if (p.getPosition() == tail)
                {
                    this.tail.getPrevious().setNext(newNode);
                    newNode.setPrevious(this.tail.getPrevious());
                    newNode.setNext(this.tail);
                    this.tail.setPrevious(newNode);
                }

                // Если не позиция после последнего, не head и не tail:
                else
                {
                    // Нужно пройтись по элементам, начиная с head, если есть элемент на такой позиции, то вставить туда
                    Node<T> currentNode = this.head;

                    while (currentNode != null)
                    {
                        if (currentNode == p.getPosition())
                        {
                            currentNode.getPrevious().setNext(newNode);
                            newNode.setPrevious(currentNode.getPrevious());
                            newNode.setNext(currentNode);
                            currentNode.setPrevious(newNode);
                        }
                        currentNode = currentNode.getNext();
                    }
                }
            }

        }

        public Position<T> Locate(T x)
        {
            // Начинаем с головы
            Node<T> current = this.head;

            // Идем по узлам пока не придем в хвост (следующий не будет null)
            while (current != null)
            {
                // Если текущий элемент - искомый - возвращаем его позицию
                if (current.getData().Equals(x))
                {
                    return new Position<T>(current);
                }
                // Переходим к следующему
                current = current.getNext();
            }
            // Если так и не нашли - возвращаем позицию после последнего
            return End();
        }

        public T Retrieve(Position<T> p)
        {
            // Переменная для искомого узла
            Node<T> searchedNode = p.getPosition();
            // Начинаем с головы
            Node<T> currentNode = this.head;
            // Идем по узлам, пока не станет null (пока не пришли в последний)
            while (currentNode != null)
            {
                // Если текущий узел - искомый, возвращаем обьект внутри узла
                if (currentNode == searchedNode)
                {
                    return currentNode.getData();
                }

                // Переходим к следующему
                currentNode = currentNode.getNext();
            }

            // Если не нашли узла в такой позиции в текущем списке - возвращаем пустой обьект
            return default(T);

        }

        public void Delete(Position<T> p)
        {
            // Проверяем, если переданная позиция - голова
            if (p.getPosition() == this.head)
            {
                // Если при этом следующего элемента нет, то список теперь пустой
                if (this.head.getNext() == null)
                {
                    this.head = null;
                    this.tail = null;
                }
                else
                {
                    // Если же есть, то узел Следующий после head теперь head
                    this.head = this.head.getNext();
                    // Теперь у этого элемента нет предыдущего
                    this.head.setPrevious(null);
                }

            }
            // если мы удаляем хвост
            else if (p.getPosition() == this.tail)
            {
                // То хвост теперь - предыдущий элемент, он точно есть
                this.tail = this.tail.getPrevious();
                // У нового хвоста теперь нет следующего элемента
                this.tail.setNext(null);
            }
            // Если и не хвост и не голова
            else
            {
                // Надо начиная с головы пройти по элементам, найти, если есть, элемент в позиции P и удалить его
                Node<T> currentNode = this.head;

                while (currentNode != null)
                {
                    // Если мы нашли элемент по такой позиции
                    if (currentNode == p.getPosition())
                    {
                        // То его предыдущий элемент теперь ведет на его следующий элемент
                        currentNode.getPrevious().setNext(currentNode.getNext());
                        // А его следующий элемент теперь ведет на его предыдущий элемент
                        currentNode.getNext().setPrevious(currentNode.getPrevious());
                        // Завершаем цикл
                        break;
                    }
                    // Если же этого не произошло, переходим к следующему
                    currentNode = currentNode.getNext();
                }
            }
        }

        public Position<T> Next(Position<T> p)
        {
            // Если нам передали хвост
            if (p.getPosition() == this.tail)
            {
                // Возвращаем позицию после последнего
                return End();
            }
            // Если же не хвост, идем от головы до нужного элемента, если он есть - возвращаем next
            else
            {
                Node<T> currentNode = this.head;
                while (currentNode != null)
                {
                    if (currentNode == p.getPosition())
                    {
                        return new Position<T>(currentNode.getNext());
                    }
                    currentNode = currentNode.getNext();
                }
                return null;
            }

        }

        public Position<T> Previous(Position<T> p)
        {
            // Идем с хвоста, пока не станет head, проверяя является ли текущий элемент p
            Node<T> currentNode = this.tail;

            while (currentNode != this.head)
            {
                // Если да, возвращаем его предыдущий элемент
                if (currentNode == p.getPosition())
                {
                    return new Position<T>(currentNode.getPrevious());
                }
                currentNode = currentNode.getPrevious();
            }
            // Если так и не нашли
            return null;
        }

        public Position<T> MakeNull()
        {
            // Делаем голову и хвост пустыми, возвразаем позицию после последнего
            this.head = this.tail = null;
            return End();
        }

        public Position<T> First()
        {
            // Если список пустой, то есть head = null, возвращаем End()
            // Если нет, возвращаем head
            return (this.head != null ? new Position<T>(this.head) : End());
        }

        public void PrintList()
        {
            Node<T> currentNode = this.head;
            while (currentNode != null)
            {
                Console.WriteLine(currentNode.getData());
                currentNode = currentNode.getNext();
            }
        }
    }
}