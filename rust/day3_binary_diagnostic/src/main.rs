mod tree;

use std::fs::File;
use std::io::{BufRead, BufReader};
use crate::tree::{BTNode};

const BITS_COUNT: usize = 12;

fn main() {
    part_1();
    part_2();
}

fn part_1() {

    let mut bits_freq = [0; BITS_COUNT];
    let mut total_numbers_count = 0;

    let lines = BufReader::new(File::open("input.txt").expect("Input file not found")).lines();
    for line in lines {
        total_numbers_count += 1;
        let string = line.unwrap();
        let mut binary_num_str = string.chars();
        for i in 0..BITS_COUNT {
            bits_freq[i] += if binary_num_str.next().unwrap() == '1' { 1 } else { 0 };
        }
        assert_eq!(None, binary_num_str.next());
    }

    let half = total_numbers_count as f32 / 2.0;
    let mut gamma_rate = 0;
    let mut epsilon_rate = 0;
    for i in 0..BITS_COUNT {
        let value = 1 << i;
        if bits_freq[i] as f32 <= half {
            gamma_rate += value;
        } else {
            epsilon_rate += value;
        }
    }
    let power_consumption = gamma_rate * epsilon_rate;

    println!("Bits frequences: {:?}; total numbers: {}, half: {}", bits_freq, total_numbers_count, half);
    println!("Gamma rate: {0:0>12b} ({0}), epsilon rate {1:0>12b} ({1})", gamma_rate, epsilon_rate);
    println!("Part 1. Power consumption: {}", power_consumption);
}

fn part_2() {

    let mut tree = BTNode::new();

    let reader = BufReader::new(File::open("input.txt").expect("Input file not found"));
    for line in reader.lines().map(|l| l.unwrap()) {
        let mut binary_num_str = line.chars(); // like "1110001011"
        let mut tree_iter = tree.as_deref_mut().unwrap();
        for _ in 0..BITS_COUNT {
            let bit_value = if binary_num_str.next().unwrap() == '1' { 1_u8 } else { 0_u8 };
            tree_iter = tree_iter.inc_count_and_move_next(bit_value);
        }
        assert_eq!(None, binary_num_str.next());
    }

    let oxygen_generator_rating = calc_rating(tree.as_deref_mut().unwrap(),
                                              |node| { node.ones_count >= node.zeroes_count });

    let co2_scrubber_rating = calc_rating(tree.as_deref_mut().unwrap(),
                                              |node| { node.ones_count < node.zeroes_count });

    println!("Oxygen generator rating: {0:0>12b} ({0}), CO2 scrubber rating {1:0>12b} ({1})",
             oxygen_generator_rating, co2_scrubber_rating);
    println!("Part 2. Life support rating is {}", oxygen_generator_rating * co2_scrubber_rating);
}

fn calc_rating<F: Fn(&BTNode) -> bool>(tree: &mut BTNode, condition: F) -> i32 {
    let mut rating = 0;
    let mut tree_iter = tree;
    for i in (0..=BITS_COUNT - 1).rev() {
        if tree_iter.ones_count + tree_iter.zeroes_count == 1 {
            if tree_iter.has_next(1) {
                rating += 1 << i;
                tree_iter = tree_iter.move_next(1);
            } else {
                tree_iter = tree_iter.move_next(0);
            }
        } else if condition(tree_iter) {
            rating += 1 << i;
            tree_iter = tree_iter.move_next(1);
        } else {
            tree_iter = tree_iter.move_next(0);
        }
    }
    rating
}