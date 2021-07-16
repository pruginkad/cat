#include <iostream>
#include <map>
#include <vector>
#include <algorithm>
#include <iomanip>
#include <fstream>
#include <string>
#include <list>


template <typename InIt, typename OutIt, typename F>
InIt split(InIt it, InIt end_it, OutIt out_it,
    F bin_func)
{
    while (it != end_it) 
    {
        auto slice_end(std::find_if_not(it, end_it,
            [](const auto& item) 
            {
                bool bRet = (item >= 'a' && item <= 'z') || (item >= 'A' && item <= 'Z');
                return bRet;
            }));

        *out_it++ = bin_func(it, slice_end);
        if (slice_end == end_it) 
        { 
            return end_it; 
        }
        it = std::next(slice_end);
    }
    return it;
}

int main(int argc, char* argv[])
{
    if (argc < 2) 
    { // We expect 3 arguments: the program name, the source path and the destination path
        std::cerr << "Usage1: " << argv[0] << " in.txt out.txt" << std::endl;
        std::cerr << "Usage2: " << argv[0] << " in.txt" << std::endl;
        return 1;
    }
    std::ifstream in;
    in.open(argv[1]);
    if (!in.is_open())
    { 
        std::cerr << "bad file: " << argv[1] << std::endl;
        return 2;
    }

    std::ofstream out(argv[2]);

    if (argc > 2)
    {
        
        if (out.is_open())
        {
            std::streambuf* coutbuf = std::cout.rdbuf(); //save old buf
            std::cout.rdbuf(out.rdbuf()); //redirect std::cout to out.txt!
        }
    }

    std::map<std::string, std::size_t> words;
    int max_word_len{ 0 };

    std::streambuf* cinbuf = std::cin.rdbuf(); //save old buf
    std::cin.rdbuf(in.rdbuf()); //redirect std::cin to in.txt!

    auto binfunc_string_maker([](auto it_a, auto it_b)
        {
            return std::string(it_a, it_b);
        });

    std::string s;
    while (std::cin >> s)
    {
        std::list<std::string> word_list;
        split(begin(s), end(s), std::back_inserter(word_list), binfunc_string_maker);

        std::for_each(word_list.begin(), word_list.end(), [&](auto& cur_word)
        { 
                if (!cur_word.empty())
                {
                    std::transform(cur_word.begin(), cur_word.end(), cur_word.begin(),
                        [](unsigned char c)
                        {
                            return std::tolower(c);
                        });
                    max_word_len = std::max<int>(max_word_len, cur_word.length());
                    ++words[cur_word];
                }                
        });
    }

    std::vector<std::pair<std::string, std::size_t>> word_counts;
    word_counts.reserve(words.size());

    std::move(std::begin(words), std::end(words), back_inserter(word_counts));
    
    // Get the most frequent words to the front
    std::sort(begin(word_counts), end(word_counts),
        [](const auto& a, const auto& b) 
        { 
            return a.second > b.second; 
        });

    
    for (const auto& elem : word_counts) 
    {
        std::cout << elem.second << " " << std::setw((std::streamsize)(max_word_len + 2)) << elem.first << '\n';
    }
}

