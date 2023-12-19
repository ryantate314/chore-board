import { createReducer } from "@reduxjs/toolkit";
import actions from '../actions/taskActions';

const taskReducer = createReducer([], (builder) => {
    builder.addCase(actions.addTaskDefinition, (state, action) => {});
});

export default taskReducer;