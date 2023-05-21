
import axios from 'axios';

axios.defaults.baseURL = "https://authserver-r3gu.onrender.com/todoitems";

export default {
  getTasks: async () => {
    console.log('get task')
    const result = await axios.get()
    console.log(result)
    return   result.data;
  },

  addTask: async (name) => {
    console.log('addTask', name)
    const result = await axios.post(``, { Name: name, IsComplete: false })
    return result.data;
  },

  setCompleted: async (id, isComplete) => {
    console.log('setCompleted', { id, isComplete })
    const result = await axios.put(`${id}`, { isComplete: isComplete })
    return result.data;
  },

  deleteTask: async (id) => {
    console.log('deleteTask')
    const result = await axios.delete(`/${id}`)
    return result.data;
  }
};