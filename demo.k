demo . {
    System
}

Main : (->) {
    n0 = node(0)
    n1 = node(1)
    n2 = node(2)
    n3 = node(3)
    n4 = node(4)
    n5 = node(5)
    n6 = node(6)

    n0.left = n1
    n0.right = n2

    n1.left = n3
    n1.right = n4

    n2.left = n5
    n2.right = n6

    print("Pre Order Traverse")
    pre_order_traverse(n0)
    print("Middle Order Traverse")
    middle_order_traverse(n0)
    print("Post Order Traverse")
    post_order_traverse(n0)

    n7 = inverse_node(n0)
    print("Inverse node")
    pre_order_traverse(n7)

    arr = {9;1;5;8;3;7;4;6;2}
    simple_sort(arr)
    arr = {9;1;5;8;3;7;4;6;2}
    bubble_sort(arr)
    arr = {9;1;5;8;3;7;4;6;2}
    quick_sort(arr)

    print("Filter Array")
    arr = filter_list(arr, {it->it > 4})
    @ ea = arr.. {
        print(ea) 
    }

    print("oop")
    app = app("test", "Windows")
    app.start()
    app.stop()
    shutdown(app)
    read()
}

pre_order_traverse : (node ?node->) {
    ? node == nil { 
        <- 
    }
    print(node.value)
    pre_order_traverse(node.left)
    pre_order_traverse(node.right)
}

post_order_traverse : (node ?node->) {
    ? node == nil { 
        <-
    }
    post_order_traverse(node.left)
    post_order_traverse(node.right)
    print(node.value)
}

middle_order_traverse : (node ?node->) {
    ? node == nil { 
        <-
    }
    middle_order_traverse(node.left)
    print(node.value)
    middle_order_traverse(node.right)
}

inverse_node : (node ?node -> node ?node) {
    ? node == nil { 
        <- nil
    }
    node.left = inverse_node(node.left)
    node.right = inverse_node(node.right)

    temp = node(node.value)
    temp.left = node.right
    temp.right = node.left
    <- temp
}

swap : (list []int, i int, j int->) {
    list[i], list[j] = list[j], list[i]
}

simple_sort : (list []int->) {
    print("Simple Sort")
    @ i = 0 ~ list.len-1.. {
        @ j = i+1 ~ list.len-1.. {
            ? list[i] > list[j] {
                swap(list, i , j)
            }
        }
    }
    @ ea = list.. { 
        print(ea) 
    }
}

bubble_sort : (list []int->) {
    print("Bubble Sort")
    @ i = 0 ~ list.len-1.. {
        @ j = list.len-2 ~~ i.. {
            ? list[j] > list[j+1] {
                swap(list, j , j+1)
            }
        }
    }
    @ ea = list.. { 
        print(ea) 
    }
}

quick_sort : (list []int->) {
    print("Quick Sort")
    quick_sort_implement(list, 0, list.len-1)
    @ ea = list.. { 
        print(ea) 
    }
}

quick_sort_implement : (list []int, low int, high int->) {
    pivot = 0
    ? low < high {
        pivot = partition(list, low, high)

        quick_sort_implement(list, low, pivot-1)
        quick_sort_implement(list, pivot+1, high)
    }
}

partition : (list []int, low int, high int -> position int) {
    pivot_key = list[low]
    
    @ low < high {
        @ low < high & list[high] >= pivot_key {
            high -= 1
        }
        swap(list, low, high)
        @ low < high & list[low] <= pivot_key {
            low += 1
        }
        swap(list, low, high)
    }

    <- low
}

filter_list : (list []int, fn (int -> bool) -> l []int) {
    filter = []int{}

    @ ea = list.. {
        ? fn(ea) {
            filter += ea
        }
    }
    <- filter
}

shutdown : (ctrl control->) {
    ctrl.shutdown()
}

node : $ {
    value int
    left ?node
    right ?node
} & (value int->$me) {
    me.value = value
}

control : $$ {
    shutdown (->)
}

program = control & $ {
    name str
    _running = false

    start : (->) {
        print("Start")
        _running = true
    }
    stop : (->) {
        print("Stop")
        _running = false
    }
    shutdown : (->) {
        print("shutdown")
        _running = false
    }
} & (name str->$me) {
    me.name = name
}

app = program & $ { 
    platform str
} & (name str, platform str->$me)(name) {
    me.platform = platform
}
