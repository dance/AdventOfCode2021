pub struct BTNode {
    zero: BTWrappedNode,
    one: BTWrappedNode,
    pub(crate) zeroes_count: i32,
    pub(crate) ones_count: i32,
}

type BTWrappedNode = Option<Box<BTNode>>;
pub type BTree = BTWrappedNode;

impl BTNode {

    pub fn new() -> BTree {
        Option::Some(Box::new(BTNode {
            zero: Option::None,
            one: Option::None,
            zeroes_count: 0,
            ones_count: 0
        }))
    }

    pub fn inc_count_and_move_next(&mut self, bit_value: u8) -> &mut BTNode {
        match bit_value {
            0 => {
                self.zeroes_count += 1;
                if self.zero.is_none() {
                    self.zero = BTNode::new();
                }
                self.move_next(0)
            },
            1 => {
                self.ones_count += 1;
                if self.one.is_none() {
                    self.one = BTNode::new();
                }
                self.move_next(1)
            },
            _ => panic!("Unexpected bit value")
        }
    }

    pub fn move_next(&mut self, bit_value: u8) -> &mut BTNode {
        match bit_value {
            0 => { self.zero.as_deref_mut().unwrap() }
            1 => { self.one.as_deref_mut().unwrap() }
            _ => panic!("Unexpected bit value")
        }
    }

    pub fn has_next(&self, bit_value: u8) -> bool {
        match bit_value {
            0 => { self.zero.is_some() }
            1 => { self.one.is_some() }
            _ => panic!("Unexpected bit value")
        }
    }

}