using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class Program
    {
        List<string> list = new List<string>();
        List<string> VN = new List<string>();
        List<string> VT = new List<string>();
        List<first> FirstVN = new List<first>();
        List<follow> FollowVN = new List<follow>();
        List<LL1M> LL1 = new List<LL1M>();
        List<string> VTLL1 = new List<string>();
        static void Main(string[] args)
        {
            Program p = new Program();
            p.copyCGF();
            p.change(p.list);
            p.check(p.list);
            p.createbase();
            p.createVNFirst();
            p.createLeftFollow();
            p.createLL1();
            Console.Write("    ");
            for (int i=0;i<p.VTLL1.Count;i++) {
                Console.Write(p.VTLL1[i]+"   ");
            }
            Console.WriteLine();
            for (int i=0;i<p.VN.Count;i++) {
                Console.Write(p.VN[i]+":");
                for (int j=0;j<p.VTLL1.Count;j++) {
                    if (p.LL1[i * p.VTLL1.Count + j].content == "")
                    {
                        Console.Write("    ");
                    }
                    else {
                        Console.Write(p.LL1[i * p.VTLL1.Count + j].content);
                    }
                }
                Console.WriteLine();
            }
            string input = "i+i*i";
            p.analyse(input);
            Console.Read();
        }
        public void copyCGF()
        {
            list.Add("E:TG");
            list.Add("G:ε|+TG");
            list.Add("T:FH");
            list.Add("H:ε|*FH");
            list.Add("F:(E)|i");
        }
        public void change(List<string> l)
        {
            for (int i=0;i<l.Count;i++) {
                if (l[i].Contains("|")) {
                    string str = l[i];
                    char[] a = str.ToCharArray();
                    int x = str.IndexOf(":");
                    string left = str.Substring(0, x+1);
                    string sub = "";
                    for (int j=x+1;j<a.Count();j++) {
                        if (a[j] == '|')
                        {
                            i = i + 1;
                            l.Insert(i,left+sub);
                            sub = "";
                        }
                        else {
                            sub = sub + a[j].ToString();
                        }
                    }
                    l.Remove(str);
                    l.Insert(i, left + sub);
                }
            }
            for (int i=0;i<l.Count;i++) {
                for(int j=i+1;j< l.Count; j++) {

                }
            }
        }
        public bool check(List<string> l)
        {

            return true;
        }
        public void createbase(){
            for (int i = 0; i < list.Count; i++)
            {
                string str = list[i];
                int x = str.IndexOf(":");
                if (!VN.Contains(str.Substring(0, x))) {
                    VN.Add(str.Substring(0, x));
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                string str = list[i];
                int x = str.IndexOf(":");
                str = str.Substring(x + 1);
                for (int j = 0; j < VN.Count; j++)
                {
                    while(str.Contains(VN[j])) {
                        str=str.Remove(str.IndexOf(VN[j]), 1);
                    }
                }
                for (int j = 0; j < str.Length;j++) {
                    if (!VT.Contains(str[j].ToString()))
                    {
                        VT.Add(str[j].ToString());
                    }
                }
            }
        }
        public void createVNFirst() {
        
            for (int i = 0; i<VN.Count;i++) {
                first f = new Lab2.first();
                f.VN = VN[i];
                FirstVN.Add(f);
            }
            for (int i=0;i<VN.Count;i++) {
                getFirst(VN[i]);
            }
        }
        public List<string> getFirst(string s) {
            List<string> copy = new List<string>();
            copy = list;
            List<string> result = new List<string>();
            for (int i = 0; i < copy.Count; i++)
            {
                string str = copy[i];
                int x = str.IndexOf(":");
                string left = str.Substring(0, x);
                string right = str.Substring(x + 1);
                if (left==s)
                {
                    if (right == "ε")
                    {
                        if (!FirstVN[VN.IndexOf(s)].firsts.Contains("ε"))
                        {
                            FirstVN[VN.IndexOf(s)].firsts.Add("ε");
                        }
                        result.Add("ε");
                    }
                    else if (isinVT(right) != -1)
                    {
                        int loc = isinVT(right);
                        if (!FirstVN[VN.IndexOf(s)].firsts.Contains(VT[loc]))
                        {
                            FirstVN[VN.IndexOf(s)].firsts.Add(VT[loc]);
                        }
                        result.Add(VT[loc]);
                    }
                    else if (isinVN(right) != -1) { 
                        List<string> sub = new List<string>();
                        string sl = right;
                        int loc = isinVN(right);
                        sub = getFirst(VN[loc]);
                        for (int k = 0; k < sub.Count; k++)
                        {
                            if (!FirstVN[VN.IndexOf(s)].firsts.Contains(sub[k]) && sub[k] != "ε")
                            {
                                FirstVN[VN.IndexOf(s)].firsts.Add(sub[k]);
                                result.Add(sub[k]);
                            }
                        }
                        sl = sl.Substring(VN[loc].Length);
                        while (sub.Contains("ε")) {
                            if (isinVT(sl)!=-1) {
                                int loc1 = isinVT(sl);
                                result.Add(VT[loc1]);
                                if (!FirstVN[VN.IndexOf(s)].firsts.Contains(VT[loc1]))
                                {
                                    FirstVN[VN.IndexOf(s)].firsts.Add(VT[loc1]);
                                    break;
                                }
                            }
                            if (isinVN(sl) != -1)
                            {
                                int loc1 = isinVN(sl);
                                List<string> sub1 = new List<string>();
                                sub1 = getFirst(VN[loc1]);
                                if (sl == VN[loc1])
                                {
                                    for (int k = 0; k < sub1.Count; k++)
                                    {
                                        if (!FirstVN[VN.IndexOf(s)].firsts.Contains(sub1[k]))
                                        {
                                            FirstVN[VN.IndexOf(s)].firsts.Add(sub1[k]);
                                        }
                                        result.Add(sub1[k]);
                                    }
                                    break;
                                }
                                else {
                                    for (int k = 0; k < sub1.Count; k++)
                                    {
                                        if (!FirstVN[VN.IndexOf(s)].firsts.Contains(sub1[k])&& sub1[k]!= "ε")
                                        {
                                            FirstVN[VN.IndexOf(s)].firsts.Add(sub1[k]);
                                        }
                                        result.Add(sub1[k]);
                                    }
                                    if (!sub1.Contains("ε")) {
                                        break;
                                    }
                                    sl = sl.Substring(VN[loc1].Length);
                                    sub = sub1;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public int isinVT(string s) {
            for (int i = 0; i < VT.Count; i++)
            {
                if (s.IndexOf(VT[i]) == 0)
                {
                    return i;
                }
            }
            return -1;
        }
        public int isinVN(string s)
        {
            for (int i = 0; i < VN.Count; i++)
            {
                if (s.IndexOf(VN[i]) == 0)
                {
                    return i;
                }
            }
            return -1;
        }
        public void createLeftFollow() {
            for (int i=0;i<VN.Count;i++) {
                follow f = new follow();
                f.VN = VN[i];
                if (i==0) {
                    f.follows.Add("$");
                }
                FollowVN.Add(f);
            }
            for (int i = 0; i < VN.Count; i++)
            {
                getFollow(VN[i]);
            }
        }
        public List<string> getFollow(string s) {
            List<string> copy = new List<string>();
            copy = list;
            List<string> result = new List<string>();
            if (s == VN[0])
            {
                result.Add("$");
            }
            for (int i = 0; i < copy.Count; i++) {
                string str = copy[i];
                int x = str.IndexOf(":");
                string left = str.Substring(0, x);
                string right = str.Substring(x + 1);
                if (right.Contains(s)) {
                    int loc = right.IndexOf(s);
                    if ((loc + s.Length) != right.Length)
                    {
                        string next = right.Substring(loc + s.Length);
                        int loct = isinVT(next);
                        int locn = isinVN(next);
                        if (loct!=-1) {
                            if (!FollowVN[VN.IndexOf(s)].follows.Contains(VT[loct]))
                            {
                                FollowVN[VN.IndexOf(s)].follows.Add(VT[loct]);
                            }
                            result.Add(VT[loct]);
                        }
                        else if (locn != -1)
                        {
                            List<string> sub = new List<string>();
                            string sl = next;
                            sub = FirstVN[locn].firsts;
                            for (int k = 0; k < sub.Count; k++)
                            {
                                if (!FollowVN[VN.IndexOf(s)].follows.Contains(sub[k]) && sub[k] != "ε")
                                {
                                    FollowVN[VN.IndexOf(s)].follows.Add(sub[k]);
                                }
                                result.Add(sub[k]);
                            }
                            sl = sl.Substring(VN[locn].Length);
                            while (sub.Contains("ε"))
                            {
                                if (sl.Length==0)
                                {
                                    if (next!=left) {
                                        List<string> sub3 = new List<string>();
                                        sub3 = getFollow(left);
                                        for (int k = 0; k < sub3.Count; k++)
                                        {
                                            if (!FollowVN[VN.IndexOf(s)].follows.Contains(sub3[k]) && sub3[k] != "ε")
                                            {
                                                FollowVN[VN.IndexOf(s)].follows.Add(sub3[k]);
                                            }
                                            result.Add(sub3[k]);
                                        }
                                    }
                                    break;
                                }
                                if (isinVT(sl) != -1)
                                {
                                    int loc1 = isinVT(sl);
                                    result.Add(VT[loc1]);
                                    if (!FollowVN[VN.IndexOf(s)].follows.Contains(VT[loc1]))
                                    {
                                        FollowVN[VN.IndexOf(s)].follows.Add(VT[loc1]);
                                        break;
                                    }
                                }else if (isinVN(sl) != -1)
                                {
                                    int loc1 = isinVN(sl);
                                    List<string> sub1 = new List<string>();
                                    sub1 = FirstVN[loc1].firsts;
                                    if (sl == VN[loc1])
                                    {
                                        for (int k = 0; k < sub1.Count; k++)
                                        {
                                            if (!FollowVN[VN.IndexOf(s)].follows.Contains(sub1[k]) && sub1[k] != "ε")
                                            {
                                                FollowVN[VN.IndexOf(s)].follows.Add(sub1[k]);
                                            }
                                            result.Add(sub1[k]);
                                        }
                                        if (sub1.Contains("ε") && (sl!=left)) {
                                            List<string> sub2 = new List<string>();
                                            sub2 = getFollow(left);
                                            for (int k = 0; k < sub2.Count; k++)
                                            {
                                                if (!FollowVN[VN.IndexOf(s)].follows.Contains(sub2[k]) && sub2[k] != "ε")
                                                {
                                                    FollowVN[VN.IndexOf(s)].follows.Add(sub2[k]);
                                                }
                                                result.Add(sub2[k]);
                                            }
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        for (int k = 0; k < sub1.Count; k++)
                                        {
                                            if (!FollowVN[VN.IndexOf(s)].follows.Contains(sub1[k]) && sub1[k] != "ε")
                                            {
                                                FollowVN[VN.IndexOf(s)].follows.Add(sub1[k]);
                                            }
                                            result.Add(sub1[k]);
                                        }
                                        if (!sub1.Contains("ε"))
                                        {
                                            break;
                                        }
                                        sl = sl.Substring(VN[loc1].Length);
                                        sub = sub1;
                                    }
                                }
                            }
                        }
                    }
                    else {
                        if (s!=left)
                        {
                            List<string> sub2 = new List<string>();
                            int locl = isinVN(left);
                            sub2 = getFollow(VN[locl]);
                            for (int k = 0; k < sub2.Count; k++)
                            {
                                if (!FollowVN[VN.IndexOf(s)].follows.Contains(sub2[k]) && sub2[k] != "ε")
                                {
                                    FollowVN[VN.IndexOf(s)].follows.Add(sub2[k]);
                                }
                                result.Add(sub2[k]);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public void createLL1() {
            for (int i=0;i<VN.Count;i++) {
                for (int j=0;j<VT.Count;j++) {
                    LL1M l = new LL1M();
                    LL1.Add(l);
                }
            }
            for (int i=0;i<VT.Count;i++) {
                VTLL1.Add(VT[i]);
            }
            VTLL1.Remove("ε");
            VTLL1.Add("$");
            List<string> copy = new List<string>();
            copy = list;
            for (int i = 0; i < copy.Count; i++) {
                string str = copy[i];
                int x = str.IndexOf(":");
                string left = str.Substring(0, x);
                string right = str.Substring(x + 1);
                List<string> first = new List<string>();
                int loct = isinVT(right);
                int locn = isinVN(right);
                int LL1VTloc = isinLL1VT(right);
                int leftloc = isinVN(left);
                if (loct != -1) {
                    if (right == "ε") {
                        for (int j=0;j<FollowVN[leftloc].follows.Count;j++) {
                            if(FollowVN[leftloc].follows[j]!= "ε")
                            {
                                int ft = isinLL1VT(FollowVN[leftloc].follows[j]);
                                LL1[leftloc * VTLL1.Count + ft].content = copy[i];
                            }
                            if (FollowVN[leftloc].follows[j] == "$") {
                                LL1[leftloc * VTLL1.Count + VTLL1.Count - 1].content = copy[i];
                            }
                        }
                    } else {
                        LL1[leftloc * VTLL1.Count + LL1VTloc].content = copy[i];
                    }
                } else if(locn!=-1){
                    List<string> first1 = FirstVN[locn].firsts;
                    for (int j = 0; j < first1.Count; j++) {
                        if (first1[j] != "ε")
                        {
                            int ft = isinLL1VT(first1[j]);
                            LL1[leftloc * VTLL1.Count + ft].content = copy[i];
                        }
                    }
                    int loc2 = locn;
                    string sr = right;
                    while (first1.Contains("ε")) {
                        if (sr.Length == VN[loc2].Length)
                        {
                            LL1[leftloc * VTLL1.Count + VN.Count - 1].content = copy[i];
                        }
                        else {
                            sr = sr.Substring(VN[loc2].Length);
                            if (isinVT(sr) != -1) {
                                loc2 = isinVT(sr);
                                LL1[leftloc * VTLL1.Count + LL1VTloc].content = copy[i];
                                break;
                            } else if (isinVN(sr) != -1)
                            {
                                loc2 = isinVN(sr);
                                List<string> first2 = FirstVN[loc2].firsts;
                                for (int j = 0; j < first2.Count; j++)
                                {
                                    if (first1[j] != "ε")
                                    {
                                        int ft = isinLL1VT(first2[j]);
                                        LL1[leftloc * VTLL1.Count + ft].content = copy[i];
                                    }
                                }
                                if (!first2.Contains("ε")) {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        public int isinLL1VT(string s)
        {
            for (int i = 0; i < VTLL1.Count; i++)
            {
                if (s.IndexOf(VTLL1[i]) == 0)
                {
                    return i;
                }
            }
            return -1;
        }
        public void CGFerror() {

        }
        public void analyse(string input) {
            List<string> stack = new List<string>();
            stack.Add("$");
            stack.Add("E");
            input = input + "$";
            char[] a = input.ToCharArray();
            int i = 0;
            for (int k = 0; k < i; k++)
            {
                Console.Write(a[k]);
            }
            for (int k = 0; k < a.Length + 3 - i; k++)
            {
                Console.Write(" ");
            }
            for (int k = 0; k < a.Length + 3 - stack.Count; k++)
            {
                Console.Write(" ");
            }
            for (int j= stack.Count-1; j>=0;j--) {
                Console.Write(stack[j]);
            }
            for (int k = 0; k < i + 2; k++)
            {
                Console.Write(" ");
            }
            for (int k = i; k < a.Length; k++)
            {
                Console.Write(a[k]);
            }
            Console.Write("            ");
            Console.WriteLine();
            while (stack.Last()!="$") {
                    if (stack.Last() == a[i].ToString())
                    {
                    int length = stack.Count;
                    stack.RemoveAt(length - 1);
                    i = i + 1;
                    for (int k = 0; k < i; k++)
                    {
                        Console.Write(a[k]);
                    }
                    for (int k = 0; k < a.Length + 3 - i; k++)
                    {
                        Console.Write(" ");
                    }
                    for (int k = 0; k < a.Length + 3 - stack.Count; k++)
                    {
                        Console.Write(" ");
                    }
                    for (int j = stack.Count - 1; j >= 0; j--)
                    {
                        Console.Write(stack[j]);
                    }
                    for (int k = 0; k < i + 2; k++)
                    {
                        Console.Write(" ");
                    }
                    for (int k = i; k < a.Length; k++)
                    {
                        Console.Write(a[k]);
                    }
                    Console.Write("  匹配  ");
                    Console.WriteLine(a[i-1]);
                    continue;
                    }
                    else if (isinVT(stack.Last()) != -1)
                    {
                        error();
                        break;
                    }
                    else if (LL1[isinVN(stack.Last()) * VTLL1.Count + isinLL1VT(a[i].ToString())].content == "")
                    {
                        error();
                        break;
                    }
                    else
                    {
                    string sentence = LL1[isinVN(stack.Last()) * VTLL1.Count + isinLL1VT(a[i].ToString())].content;
                    int length = stack.Count;
                        stack.RemoveAt(length - 1);
                        int x = sentence.IndexOf(":");
                        string right = sentence.Substring(x + 1);
                        int locn = isinVNend(right);
                        int loct = isinVTend(right);
                        while (right.Length > 0)
                        {
                        if (loct != -1)
                            {
                                if (VT[loct] == "ε")
                                {
                                    right = right.Substring(0, right.Length - VT[loct].Length);
                                }
                                else
                                {
                                    stack.Add(VT[loct]);
                                    right = right.Substring(0, right.Length - VT[loct].Length);
                                    if (right.Length == 0)
                                    {
                                        break;
                                    }
                                    locn = isinVNend(right);
                                    loct = isinVTend(right);
                                }
                            }
                            else if (locn != -1)
                            {

                                stack.Add(VN[locn]);
                                right = right.Substring(0, right.Length - VN[locn].Length);
                                if (right.Length == 0)
                                {
                                    break;
                                }
                                locn = isinVNend(right);
                                loct = isinVTend(right);
                            }
                        }
                    for (int k = 0; k < i; k++)
                    {
                        Console.Write(a[k]);
                    }
                    for (int k = 0; k < a.Length + 3 - i; k++)
                    {
                        Console.Write(" ");
                    }
                    for (int k = 0; k < a.Length + 3 - stack.Count; k++)
                    {
                        Console.Write(" ");
                    }
                    for (int j = stack.Count - 1; j >= 0; j--)
                    {
                        Console.Write(stack[j]);
                    }
                    for (int k = 0; k < i + 2; k++)
                    {
                        Console.Write(" ");
                    }
                    for (int k = i; k < a.Length; k++)
                    {
                        Console.Write(a[k]);
                    }
                    Console.Write("  输出  ");
                    Console.WriteLine(sentence);
                }
            }
        }
        public void error() {

        }
        public int isinVTend(string s)
        {
            for (int i = 0; i < VT.Count; i++)
            {
                if (s.IndexOf(VT[i]) == (s.Length- VT[i].Length))
                {
                    return i;
                }
            }
            return -1;
        }
        public int isinVNend(string s)
        {
            for (int i = 0; i < VN.Count; i++)
            {
                if (s.IndexOf(VN[i]) == (s.Length - VN[i].Length))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
