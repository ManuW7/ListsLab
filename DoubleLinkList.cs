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
            string nameStr = new string(Name).TrimEnd('\0');
            string addressStr = new string(Address).TrimEnd('\0');
            return $"Name: {nameStr}, Address: {addressStr}";
        }
    }

    public class Node<T>
    {
        public T data;
        public Node<T>? previous;
        public Node<T>? next;

        public Node(T data)
        {
            this.data = data;
            previous = null;
            next = null;
        }



    }

    public class Position<T>
    {
        public Node<T>? position;


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

        //Вспомогательные методы

        // Проверка, есть ли такой элемент в списке
        private bool PositionIsValid(Position<T> p)
        {
            Node<T> currentNode = this.head;
            while (currentNode != null) {
                if (currentNode == p.position)
                {
                    return true;
                }
                currentNode = currentNode.next;
            }
            return false;
        }

        public Position<T> End()
        {
            return new Position<T>(null);
        }

        public void Insert(Position<T> p, T x)
        {
            Node<T> newNode = new Node<T>(x);


            // Если переданная позиция - это позиция после последнего
            if (p.position == null)
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
                    newNode.previous = (this.tail);
                    this.tail.next = (newNode);
                    this.tail = newNode;
                }
            }
            // Если переданная позиция - не позиция после последнего
            else
            {
                // Если вставляем в head
                if (p.position == head)
                {
                    this.head.previous = (newNode);
                    newNode.next = (this.head);
                    this.head = newNode;
                }
                // Если вставляем в tail
                else if (p.position == tail)
                {
                    this.tail.previous.next = (newNode);
                    newNode.previous = (this.tail.previous);
                    newNode.next = (this.tail);
                    this.tail.previous = (newNode);
                }

                // Если не позиция после последнего, не head и не tail:
                else
                {
                    // Если в списке есть такая позиция
                    if (this.PositionIsValid(p)){
                        // В предыдущий для p элемент записываем ссылку на вставялемый элемент
                        p.position.previous.next = newNode;
                        // Во вставляемый узел записываем ссылку на предыдущий элемент
                        newNode.previous = (p.position.previous);
                        // В следующий для всталяемого элемента вставляем элемент, который был на позиции p
                        newNode.next = p.position;
                        // В предыдущий для элемента по адресу p записываем новый
                        p.position.previous = newNode;
                    }
                }
            }

        }

        public Position<T> Locate(T x)
        {
            // Начинаем с головы
            Node<T> current = this.head;

            // Идем по узлам пока не придем в хвост
            while (current != null)
            {
                // Если текущий элемент - искомый - возвращаем его позицию
                if (current.data.Equals(x))
                {
                    return new Position<T>(current);
                }
                // Переходим к следующему
                current = current.next;
            }
            // Если так и не нашли - возвращаем позицию после последнего
            return End();
        }



        public T Retrieve(Position<T> p)
        {
            // Проверяем, если есть в списке элемент с такой позицией 
            if (this.PositionIsValid(p))
            {
                // Если есть, возвращаем его содержимое
                return p.position.data;
            }
            // Если нет
            return default(T);

        }

        public void Delete(Position<T> p)
        {
            // Проверяем, если переданная позиция - голова
            if (p.position == this.head)
            {
                // Если при этом следующего элемента нет, то список теперь пустой
                if (this.head.next == null)
                {
                    this.head = null;
                    this.tail = null;
                }
                else
                {
                    // Если же есть, то узел Следующий после head теперь head
                    this.head = this.head.next;
                    // Теперь у этого элемента нет предыдущего
                    this.head.previous = (null);
                }

            }
            // если мы удаляем хвост
            else if (p.position == this.tail)
            {
                // То хвост теперь - предыдущий элемент, он точно есть
                this.tail = this.tail.previous;
                // У нового хвоста теперь нет следующего элемента
                this.tail.next = (null);
            }
            // Если и не хвост и не голова
            else
            {
                // Проверяем, есть ли такая позиция в списке
                if (this.PositionIsValid(p))
                {
                    // Если есть, то элемент, предыдущий для нее, теперь ведет на элемент следующий для нее и наоборот
                    p.position.previous.next = p.position.next;
                    p.position.next.previous = p.position.previous;
                }

            }
        }

        public Position<T> Next(Position<T> p)
        {
            // Если нам передали хвост
            if (p.position == this.tail)
            {
                // Возвращаем позицию после последнего
                return End();
            }
            // Если же не хвост, идем от головы до нужного элемента, если он есть - возвращаем next
            else
            {
                // Если такой элемент есть
                if (this.PositionIsValid(p))
                {
                    return new Position<T>(p.position.next);
                }
            }

            return null;
        }

        public Position<T> Previous(Position<T> p)
        {
            // В списке есть такая позиция? 
            if (this.PositionIsValid(p))
            {
                return new Position<T>(p.position.previous);
            }
            // Если так и не нашли
            return null;
        }

        public Position<T> MakeNull()
        {
            // Делаем голову и хвост пустыми, возвращаем позицию после последнего
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
                Console.WriteLine(currentNode.data);
                currentNode = currentNode.next;
            }
        }
    }
}