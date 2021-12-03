use std::fs::File;
use std::io::{BufRead, BufReader};

fn main() {
    let mut previous_depth = 0;
    let mut depth_increased_count = -1;

    let lines = BufReader::new(File::open("input.txt").expect("Input file not found")).lines();
    for line in lines {
        let current_depth = line.expect("expected string").parse::<i32>().unwrap();
        if current_depth > previous_depth {
            depth_increased_count += 1;
        }
        previous_depth = current_depth;
    }

    println!("Depth increased times: {}", depth_increased_count);
}